using System.Text.Json;
using BitirmeBackend.Application.Interfaces;
using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Application.Interfaces.Services;
using BitirmeBackend.Contracts.Dtos;
using BitirmeBackend.Contracts.Requests;
using BitirmeBackend.Contracts.Responses;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Domain.Enums;

namespace BitirmeBackend.Application.Services;

public class ActionPlanService : IActionPlanService
{
    private readonly IAssessmentRepository _assessments;
    private readonly IActionPlanRepository _actionPlans;
    private readonly IAiPredictionRepository _aiPredictions;
    private readonly IActionCatalogRepository _actionCatalog;
    private readonly IModelVersionRepository _modelVersions;
    private readonly IEmployeeService _employeeService;
    private readonly IMlPredictionClient _mlClient;
    private readonly IEmployeeTaskRepository _employeeTasks;
    private readonly ICompetencyLabelResolver _competencyLabels;
    private readonly ILlmReportService _llmReportService;
    private readonly IUnitOfWork _uow;

    public ActionPlanService(
        IAssessmentRepository assessments,
        IActionPlanRepository actionPlans,
        IAiPredictionRepository aiPredictions,
        IActionCatalogRepository actionCatalog,
        IModelVersionRepository modelVersions,
        IEmployeeService employeeService,
        IMlPredictionClient mlClient,
        IEmployeeTaskRepository employeeTasks,
        ICompetencyLabelResolver competencyLabels,
        ILlmReportService llmReportService,
        IUnitOfWork uow)
    {
        _assessments      = assessments;
        _actionPlans      = actionPlans;
        _aiPredictions    = aiPredictions;
        _actionCatalog    = actionCatalog;
        _modelVersions    = modelVersions;
        _employeeService  = employeeService;
        _mlClient         = mlClient;
        _employeeTasks    = employeeTasks;
        _competencyLabels = competencyLabels;
        _llmReportService = llmReportService;
        _uow              = uow;
    }

    public async Task<ActionPlanDetailDto> GenerateDraftActionPlanAsync(GenerateActionPlanRequest request, int requestingUserId)
    {
        // Step 1 & 2: Load assessment and employee (employee loaded via assessment.EmployeeId)
        var assessment = await _assessments.GetByIdAsync(request.AssessmentId)
            ?? throw new KeyNotFoundException($"Değerlendirme bulunamadı: {request.AssessmentId}");

        // Assessment must be Completed before a plan can be generated from it
        if (assessment.Status != AssessmentStatus.Completed)
            throw new ArgumentException("Değerlendirme tamamlanmadan gelişim planı üretilemez.");

        var employeeId = assessment.EmployeeId;

        // Step 3: Duplicate active plan check — BEFORE ML call
        var existingPlan = await _actionPlans.GetActiveByAssessmentIdAsync(request.AssessmentId);
        if (existingPlan is not null)
            throw new ArgumentException("Bu assessment için zaten aktif bir aksiyon planı mevcut.");

        // Step 3b: Active incomplete plan check across ANY assessment for this employee
        var employeePlans = await _actionPlans.GetByEmployeeIdAsync(employeeId);
        var activeIncompletePlan = employeePlans.FirstOrDefault(p =>
            p.Status != ActionPlanStatus.Completed && p.Status != ActionPlanStatus.Cancelled);
        if (activeIncompletePlan is not null)
            throw new ArgumentException("Çalışanın devam eden tamamlanmamış bir gelişim planı bulunmaktadır. Yeni bir plan oluşturulamaz.");

        // Step 4: ML health check
        var isHealthy = await _mlClient.IsHealthyAsync();
        if (!isHealthy)
            throw new InvalidOperationException("ML servisi hazır değil.");

        // Step 5 & 6: Build features and ML request
        var features = await _employeeService.GetEmployeeFeaturesForPredictionAsync(employeeId, request.AssessmentId);
        var mlRequest = new MlPredictionRequest
        {
            EmployeeId = employeeId,
            Features   = BuildFeatureDictionary(features)
        };

        // Start transaction — everything from here must be atomic
        await _uow.BeginTransactionAsync();
        try
        {
            // Step 8: Load active model version (before ML call so it's in scope)
            var modelVersion = await _modelVersions.GetActiveAsync();
            int modelVersionId = modelVersion?.Id ?? 1;

            // Step 7: Call ML service
            MlPredictionResponse mlResponse;
            try
            {
                mlResponse = await _mlClient.PredictActionsTopKAsync(mlRequest, request.TopK);
            }
            catch (Exception ex)
            {

                var failedRun = new AiPredictionRun
                {
                    AssessmentId       = request.AssessmentId,
                    ModelVersionId     = modelVersionId,
                    RequestedByUserId  = requestingUserId,
                    Status             = PredictionRunStatus.Failed,
                    ErrorMessage       = ex.Message
                };
                await _aiPredictions.AddRunAsync(failedRun);
                await _uow.SaveChangesAsync();
                await _uow.CommitAsync();
                throw;
            }

            // Guard: ML returned no recommendations — do not persist an empty plan
            // that would permanently block the employee from new plans/assessments.
            if (mlResponse.RecommendedActions == null || !mlResponse.RecommendedActions.Any())
                throw new InvalidOperationException("ML servisi hiç aksiyon önerisi döndürmedi. Plan oluşturulamaz.");

            // Step 9: Save successful prediction run
            var predictionRun = new AiPredictionRun
            {
                AssessmentId      = request.AssessmentId,
                ModelVersionId    = modelVersionId,
                RequestedByUserId = requestingUserId,
                Status            = PredictionRunStatus.Success
            };
            await _aiPredictions.AddRunAsync(predictionRun);
            await _uow.SaveChangesAsync();

            // Step 10: Save predicted actions
            var recommendedActions = mlResponse.RecommendedActions
                .Select((a, i) => (dto: a, rank: i + 1))
                .ToList();

            var predictedActions = recommendedActions.Select(x => new AiPredictedAction
            {
                PredictionRunId = predictionRun.Id,
                ActionCode      = x.dto.Code,
                Probability     = x.dto.Probability,
                RankOrder       = x.rank,
                IsSelected      = true
            }).ToList();

            await _aiPredictions.AddPredictedActionsAsync(predictedActions);
            await _uow.SaveChangesAsync();

            // Step 11: Bulk load action catalog entries (keyed by string ActionId)
            var codes   = recommendedActions.Select(x => x.dto.Code);
            var catalog = (await _actionCatalog.GetByCodesAsync(codes))
                .ToDictionary(c => c.ActionId, StringComparer.OrdinalIgnoreCase);

            // Step 12: Create action plan
            var plan = new ActionPlan
            {
                AssessmentId     = request.AssessmentId,
                EmployeeId       = employeeId,
                CreatedByUserId  = requestingUserId,
                Status           = ActionPlanStatus.Draft
            };
            await _actionPlans.AddAsync(plan);
            await _uow.SaveChangesAsync();

            // Step 13: Create action plan items.
            // Title/Description/Resource/DeliveryType are parsed from ActionCatalog.ContentData
            // (JSONB) and snapshotted onto the item so future catalog edits don't alter the plan.
            var createdItems = new List<ActionPlanItem>();
            foreach (var (action, rank) in recommendedActions)
            {
                var predicted = predictedActions.First(p => p.ActionCode == action.Code);
                catalog.TryGetValue(action.Code, out var entry);

                var competencyLabel = _competencyLabels.Resolve(
                    entry?.TargetCompetency, features.Department, features.JobRole);
                var snapshot = ResolveContent(
                    entry, action.Code, features.JobRole, features.Department, competencyLabel);

                var item = new ActionPlanItem
                {
                    ActionPlanId        = plan.Id,
                    ActionCatalogId     = entry?.ActionId,
                    AiPredictedActionId = predicted.Id,
                    Title               = snapshot.Title,
                    Description         = snapshot.Description,
                    Resource            = snapshot.Resource,
                    DeliveryType        = snapshot.DeliveryType,
                    // FIX 6: dynamic priority instead of a static Medium. The ML response is
                    // ranked by relevance (rank 1 = weakest competency / most needed action),
                    // so the top tier maps to High, the middle to Medium, the rest to Low.
                    Priority            = DerivePriority(rank, recommendedActions.Count),
                    Source              = ActionPlanItemSource.AI,
                    OrderNo             = rank
                };
                await _actionPlans.AddItemAsync(item);
                createdItems.Add(item);
            }

            // Step 13b: Generate the LLM evaluation summary ONCE and store it on the plan,
            // so PDF export can reuse it without calling the LLM again. Best-effort —
            // any failure here must NOT abort plan generation (AiSummary stays empty).
            plan.AiSummary = await BuildAiSummaryAsync(request.AssessmentId, employeeId, features, createdItems);
            _actionPlans.Update(plan);

            // Step 14: Commit
            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();

            // Step 15: Return detail DTO
            return await GetActionPlanByIdAsync(plan.Id);
        }
        catch (ArgumentException)
        {
            await _uow.RollbackAsync();
            throw;
        }
        catch (InvalidOperationException)
        {
            await _uow.RollbackAsync();
            throw;
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            await _uow.RollbackAsync();
            throw;
        }
    }

    public async Task<ActionPlanDetailDto> GetActionPlanByIdAsync(int id)
    {
        var plan = await _actionPlans.GetByIdWithItemsAsync(id)
            ?? throw new KeyNotFoundException($"Aksiyon planı bulunamadı: {id}");
        return ToDetailDto(plan);
    }

    public async Task<List<ActionPlanDetailDto>> GetEmployeeActionPlansAsync(int employeeId)
    {
        var plans = await _actionPlans.GetByEmployeeIdWithItemsAsync(employeeId);
        return plans.Select(ToDetailDto).ToList();
    }

    public async Task<ActionPlanItemDto> UpdateActionPlanItemAsync(int planId, int itemId, UpdateActionPlanItemRequest request)
    {
        var plan = await _actionPlans.GetByIdAsync(planId)
            ?? throw new KeyNotFoundException($"Aksiyon planı bulunamadı: {planId}");

        // Only Draft/Edited plans may be modified
        if (plan.Status != ActionPlanStatus.Draft && plan.Status != ActionPlanStatus.Edited)
            throw new ArgumentException("Onaylanmış veya gönderilmiş planda değişiklik yapılamaz.");

        var item = await _actionPlans.GetItemByIdAsync(itemId)
            ?? throw new KeyNotFoundException($"Aksiyon planı kalemi bulunamadı: {itemId}");

        if (item.ActionPlanId != planId)
            throw new ArgumentException("Bu kalem belirtilen plana ait değil.");

        // Update provided fields
        if (!string.IsNullOrWhiteSpace(request.Title))
            item.Title = request.Title;

        if (!string.IsNullOrWhiteSpace(request.Description))
            item.Description = request.Description;

        if (!string.IsNullOrWhiteSpace(request.Priority) &&
            Enum.TryParse<PriorityLevel>(request.Priority, ignoreCase: true, out var priority))
            item.Priority = priority;

        if (request.DueDate.HasValue)
            item.DueDate = request.DueDate;

        if (request.OrderNo > 0)
            item.OrderNo = request.OrderNo;

        // Source rule: AI → EditedAI; Manual/EditedAI → keep
        if (item.Source == ActionPlanItemSource.AI)
            item.Source = ActionPlanItemSource.EditedAI;

        item.UpdatedAt = DateTime.UtcNow;
        _actionPlans.UpdateItem(item);
        await _uow.SaveChangesAsync();

        return ToItemDto(item);
    }

    public async Task<ActionPlanItemDto> AddManualItemAsync(int planId, AddManualActionPlanItemRequest request, int requestingUserId)
    {
        var plan = await _actionPlans.GetByIdWithItemsAsync(planId)
            ?? throw new KeyNotFoundException($"Aksiyon planı bulunamadı: {planId}");

        if (plan.Status != ActionPlanStatus.Draft && plan.Status != ActionPlanStatus.Edited)
            throw new ArgumentException($"Plan yalnızca Draft veya Edited durumunda düzenlenebilir. Mevcut durum: {plan.Status}");

        var maxOrder = plan.Items.Any() ? plan.Items.Max(i => i.OrderNo) : 0;

        if (!Enum.TryParse<PriorityLevel>(request.Priority, ignoreCase: true, out var priority))
            priority = PriorityLevel.Medium;

        var item = new ActionPlanItem
        {
            ActionPlanId        = planId,
            ActionCatalogId     = request.ActionCatalogId,
            AiPredictedActionId = null,
            Title               = request.Title,
            Description         = request.Description,
            Priority            = priority,
            DueDate             = request.DueDate,
            Source              = ActionPlanItemSource.Manual,
            OrderNo             = maxOrder + 1
        };

        await _actionPlans.AddItemAsync(item);

        // Transition Draft → Edited
        if (plan.Status == ActionPlanStatus.Draft)
        {
            plan.Status = ActionPlanStatus.Edited;
            _actionPlans.Update(plan);
        }

        await _uow.SaveChangesAsync();

        return ToItemDto(item);
    }

    public async Task RemoveItemAsync(int planId, int itemId)
    {
        var plan = await _actionPlans.GetByIdAsync(planId)
            ?? throw new KeyNotFoundException($"Aksiyon planı bulunamadı: {planId}");

        // Only Draft/Edited plans may be modified
        if (plan.Status != ActionPlanStatus.Draft && plan.Status != ActionPlanStatus.Edited)
            throw new ArgumentException("Onaylanmış veya gönderilmiş planda değişiklik yapılamaz.");

        var item = await _actionPlans.GetItemByIdAsync(itemId)
            ?? throw new KeyNotFoundException($"Aksiyon planı kalemi bulunamadı: {itemId}");

        if (item.ActionPlanId != planId)
            throw new ArgumentException("Bu kalem belirtilen plana ait değil.");

        // Soft delete
        _actionPlans.RemoveItem(item);

        // Transition Draft → Edited
        if (plan.Status == ActionPlanStatus.Draft)
        {
            plan.Status = ActionPlanStatus.Edited;
            _actionPlans.Update(plan);
        }

        await _uow.SaveChangesAsync();
    }

    public async Task<ActionPlanDetailDto> ApproveActionPlanAsync(int id, int requestingUserId)
    {
        var plan = await _actionPlans.GetByIdWithItemsAsync(id)
            ?? throw new KeyNotFoundException($"Aksiyon planı bulunamadı: {id}");

        // Only Draft/Edited (or an already-Approved idempotent call) may be approved.
        // Blocks Sent/Completed/Cancelled from being (re-)approved.
        if (plan.Status != ActionPlanStatus.Draft &&
            plan.Status != ActionPlanStatus.Edited &&
            plan.Status != ActionPlanStatus.Approved)
            throw new ArgumentException($"Bu plan onaylanamaz. Mevcut durum: {plan.Status}");

        // Idempotent: already approved → return current state
        if (plan.Status == ActionPlanStatus.Approved)
            return ToDetailDto(plan);

        // Validate non-empty
        var activeItems = plan.Items.Where(i => !i.IsDeleted).ToList();
        if (activeItems.Count == 0)
            throw new ArgumentException("Boş plan onaylanamaz.");

        plan.Status     = ActionPlanStatus.Approved;
        plan.ApprovedAt = DateTime.UtcNow;
        _actionPlans.Update(plan);
        await _uow.SaveChangesAsync();

        return await GetActionPlanByIdAsync(id);
    }

    public async Task<ActionPlanDetailDto> SendActionPlanToEmployeeAsync(int id, int requestingUserId)
    {
        var plan = await _actionPlans.GetByIdWithItemsAsync(id)
            ?? throw new KeyNotFoundException($"Aksiyon planı bulunamadı: {id}");

        // Idempotent: already sent → return current state
        if (plan.Status == ActionPlanStatus.Sent)
            return ToDetailDto(plan);

        if (plan.Status != ActionPlanStatus.Approved)
            throw new ArgumentException($"Plan onaylanmış olmadan gönderilemez. Mevcut durum: {plan.Status}");

        await _uow.BeginTransactionAsync();
        try
        {
            plan.Status = ActionPlanStatus.Sent;
            plan.SentAt = DateTime.UtcNow;
            _actionPlans.Update(plan);

            var activeItems = plan.Items.Where(i => !i.IsDeleted).ToList();
            foreach (var item in activeItems)
            {
                var existingTasks = await _employeeTasks.GetByActionPlanItemIdAsync(item.Id);
                if (existingTasks.Any())
                    continue; // idempotency — skip if task already exists

                var task = new EmployeeTask
                {
                    ActionPlanItemId  = item.Id,
                    EmployeeId        = plan.EmployeeId,
                    AssignedByUserId  = requestingUserId,
                    Status            = EmployeeTaskStatus.Pending,
                    AssignedAt        = DateTime.UtcNow,
                    DueDate           = item.DueDate
                };
                await _employeeTasks.AddAsync(task);
            }

            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }

        return await GetActionPlanByIdAsync(id);
    }

    public async Task<ActionPlanDetailDto> CancelActionPlanAsync(int id, int requestingUserId)
    {
        var plan = await _actionPlans.GetByIdWithItemsAsync(id)
            ?? throw new KeyNotFoundException($"Aksiyon planı bulunamadı: {id}");

        // Idempotent: already cancelled → return current state
        if (plan.Status == ActionPlanStatus.Cancelled)
            return ToDetailDto(plan);

        if (plan.Status == ActionPlanStatus.Completed)
            throw new ArgumentException("Tamamlanmış plan iptal edilemez.");

        // Only Draft/Edited plans may be cancelled; Approved/Sent cannot.
        if (plan.Status != ActionPlanStatus.Draft && plan.Status != ActionPlanStatus.Edited)
            throw new ArgumentException("Onaylanmış veya gönderilmiş plan iptal edilemez. Lütfen yöneticinizle iletişime geçin.");

        plan.Status    = ActionPlanStatus.Cancelled;
        plan.UpdatedAt = DateTime.UtcNow;
        _actionPlans.Update(plan);
        await _uow.SaveChangesAsync();

        return await GetActionPlanByIdAsync(id);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    // Builds the LLM evaluation summary for a freshly created plan. Fully defensive:
    // any failure (missing data, LLM error) yields an empty string instead of throwing,
    // so plan generation is never blocked by the summary.
    private async Task<string> BuildAiSummaryAsync(
        int assessmentId, int employeeId, EmployeeFeatureDto features, List<ActionPlanItem> createdItems)
    {
        try
        {
            var scores = await _assessments.GetScoresByAssessmentIdAsync(assessmentId);
            var competencyScores = (scores ?? Enumerable.Empty<AssessmentScore>())
                .Where(s => !s.IsDeleted)
                .GroupBy(s => new { s.CompetencyId, Name = s.Competency?.Name ?? string.Empty })
                .Select(g => (CompetencyName: g.Key.Name, Score: g.Average(s => s.Score)))
                .Where(x => !string.IsNullOrWhiteSpace(x.CompetencyName))
                .ToList();

            var actionItemTitles = createdItems.Select(i => i.Title).ToList();

            string employeeName;
            try
            {
                var employeeDetail = await _employeeService.GetEmployeeByIdAsync(employeeId);
                employeeName = employeeDetail?.FullName ?? string.Empty;
            }
            catch
            {
                employeeName = string.Empty;
            }

            return await _llmReportService.GenerateActionPlanSummaryAsync(
                employeeName, features.Department, features.JobRole, competencyScores, actionItemTitles)
                ?? string.Empty;
        }
        catch
        {
            // Summary is non-critical — keep the plan, drop the summary.
            return string.Empty;
        }
    }

    // Maps a 1-based ML rank to a priority tier: the top third of recommendations is High,
    // the middle third Medium, the rest Low. Rank 1 is the weakest competency / most-needed action.
    private static PriorityLevel DerivePriority(int rank, int total)
    {
        if (total <= 0)
            return PriorityLevel.Medium;

        var third = (int)Math.Ceiling(total / 3.0);
        if (rank <= third)
            return PriorityLevel.High;
        if (rank <= third * 2)
            return PriorityLevel.Medium;
        return PriorityLevel.Low;
    }

    private static Dictionary<string, object> BuildFeatureDictionary(EmployeeFeatureDto f) => new()
    {
        ["Age"]                     = f.Age,
        ["Attrition"]               = f.Attrition,
        ["BusinessTravel"]          = f.BusinessTravel,
        ["Department"]              = f.Department,
        ["Education"]               = int.TryParse(f.Education, out var edu) ? edu : 3,
        ["EducationField"]          = f.EducationField,
        ["EnvironmentSatisfaction"] = f.EnvironmentSatisfaction,
        ["Gender"]                  = f.Gender,
        ["JobRole"]                 = f.JobRole,
        ["JobSatisfaction"]         = f.JobSatisfaction,
        ["MaritalStatus"]           = f.MaritalStatus,
        ["WorkLifeBalance"]         = f.WorkLifeBalance,
        ["TotalWorkingYears"]       = f.TotalWorkingYears,
        ["YearsAtCompany"]          = f.YearsAtCompany,
        ["YearsInCurrentRole"]      = f.YearsInCurrentRole,
        ["YearsWithCurrManager"]    = f.YearsWithCurrManager,
        ["PerformanceScore"]        = f.PerformanceScore,
        ["Core_Communication"]      = f.Core_Communication,
        ["Core_Teamwork"]           = f.Core_Teamwork,
        ["Core_ProblemSolving"]     = f.Core_ProblemSolving,
        ["Core_Adaptability"]       = f.Core_Adaptability,
        ["Core_Initiative"]         = f.Core_Initiative,
        ["Core_Accountability"]     = f.Core_Accountability,
        ["Core_LearningAgility"]    = f.Core_LearningAgility,
        ["Core_TimeManagement"]     = f.Core_TimeManagement,
        ["Dept_Comp1"]              = f.Dept_Comp1,
        ["Dept_Comp2"]              = f.Dept_Comp2,
        ["Dept_Comp3"]              = f.Dept_Comp3,
        ["Role_Comp1"]              = f.Role_Comp1,
        ["Role_Comp2"]              = f.Role_Comp2,
    };

    private static ActionPlanDetailDto ToDetailDto(ActionPlan plan) => new()
    {
        Id               = plan.Id,
        AssessmentId     = plan.AssessmentId,
        EmployeeId       = plan.EmployeeId,
        EmployeeName     = plan.Employee?.FullName ?? string.Empty,
        CreatedByUserId  = plan.CreatedByUserId,
        CreatedByUserName = plan.CreatedByUser?.FullName ?? string.Empty,
        Status           = plan.Status.ToString(),
        ApprovedAt       = plan.ApprovedAt,
        SentAt           = plan.SentAt,
        CreatedAt        = plan.CreatedAt,
        UpdatedAt        = plan.UpdatedAt,
        Items            = plan.Items
            .Where(i => !i.IsDeleted)
            .OrderBy(i => i.EmployeeTasks.Any(t => !t.IsDeleted && t.Status == EmployeeTaskStatus.Completed) ? 1 : 0)
            .ThenByDescending(i => i.Priority)
            .ThenBy(i => i.OrderNo)
            .Select(ToItemDto)
    };

    private static ActionPlanItemDto ToItemDto(ActionPlanItem item) => new()
    {
        Id                  = item.Id,
        ActionPlanId        = item.ActionPlanId,
        ActionCatalogId     = item.ActionCatalogId,
        AiPredictedActionId = item.AiPredictedActionId,
        Title               = item.Title,
        Description         = item.Description,
        Resource            = item.Resource,
        DeliveryType        = item.DeliveryType,
        Priority            = item.Priority.ToString(),
        DueDate             = item.DueDate,
        Source              = item.Source.ToString(),
        OrderNo             = item.OrderNo,
        TaskStatus          = item.EmployeeTasks?.FirstOrDefault(t => !t.IsDeleted)?.Status.ToString()
    };

    // ── ContentData (JSONB) parsing ─────────────────────────────────────────────
    // Core actions:        flat object { title, description, resource, delivery_type }
    // Department/Role:      nested object keyed by JobRole / Department name
    //                       → try JobRole, then Department, then first object value
    // Anything unparseable: documented fallback title/description

    private static (string Title, string Description, string? Resource, string? DeliveryType) ResolveContent(
        ActionCatalog? entry, string actionCode, string jobRole, string department, string competencyLabel)
    {
        var fallbackTitle = $"Gelişim Aksiyonu: {actionCode}";
        const string fallbackDescription = "Bu aksiyon için lütfen yöneticinizle iletişime geçin.";

        if (entry is null || string.IsNullOrWhiteSpace(entry.ContentData))
            return (fallbackTitle, fallbackDescription, null, null);

        try
        {
            using var doc = JsonDocument.Parse(entry.ContentData);
            var root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                return (fallbackTitle, fallbackDescription, null, null);

            JsonElement content;
            if (root.TryGetProperty("title", out _))
                content = root;                                   // flat core content
            else if (TryGetObjectProperty(root, jobRole, out var byRole))
                content = byRole;                                 // nested — role match
            else if (TryGetObjectProperty(root, department, out var byDept))
                content = byDept;                                 // nested — department match
            else
            {
                content = FirstObject(root);                      // nested — first available
                if (content.ValueKind != JsonValueKind.Object)
                    return (fallbackTitle, fallbackDescription, null, null);
            }

            var title    = GetString(content, "title");
            var resource = GetString(content, "resource");
            var delivery = GetString(content, "delivery_type");
            var description = GetString(content, "description");

            if (string.IsNullOrWhiteSpace(title))
                return (fallbackTitle, fallbackDescription, resource, delivery);

            if (string.IsNullOrWhiteSpace(description))
                description = $"{competencyLabel} yetkinliğini geliştirmeye yönelik {entry.ActionType} aksiyonu.";

            return (title!, description!, resource, delivery);
        }
        catch (JsonException)
        {
            return (fallbackTitle, fallbackDescription, null, null);
        }
    }

    private static bool TryGetObjectProperty(JsonElement root, string? name, out JsonElement value)
    {
        value = default;
        if (string.IsNullOrWhiteSpace(name))
            return false;

        foreach (var prop in root.EnumerateObject())
        {
            if (prop.Value.ValueKind == JsonValueKind.Object &&
                string.Equals(prop.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                value = prop.Value;
                return true;
            }
        }
        return false;
    }

    private static JsonElement FirstObject(JsonElement root)
    {
        foreach (var prop in root.EnumerateObject())
            if (prop.Value.ValueKind == JsonValueKind.Object)
                return prop.Value;
        return default;
    }

    private static string? GetString(JsonElement obj, string propName) =>
        obj.ValueKind == JsonValueKind.Object &&
        obj.TryGetProperty(propName, out var el) &&
        el.ValueKind == JsonValueKind.String
            ? el.GetString()
            : null;
}
