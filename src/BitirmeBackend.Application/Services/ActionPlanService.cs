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
    private readonly IUnitOfWork _uow;

    public ActionPlanService(
        IAssessmentRepository assessments,
        IActionPlanRepository actionPlans,
        IAiPredictionRepository aiPredictions,
        IActionCatalogRepository actionCatalog,
        IModelVersionRepository modelVersions,
        IEmployeeService employeeService,
        IMlPredictionClient mlClient,
        IUnitOfWork uow)
    {
        _assessments    = assessments;
        _actionPlans    = actionPlans;
        _aiPredictions  = aiPredictions;
        _actionCatalog  = actionCatalog;
        _modelVersions  = modelVersions;
        _employeeService = employeeService;
        _mlClient       = mlClient;
        _uow            = uow;
    }

    public async Task<ActionPlanDetailDto> GenerateDraftActionPlanAsync(GenerateActionPlanRequest request, int requestingUserId)
    {
        // Step 1 & 2: Load assessment and employee (employee loaded via assessment.EmployeeId)
        var assessment = await _assessments.GetByIdAsync(request.AssessmentId)
            ?? throw new KeyNotFoundException($"Değerlendirme bulunamadı: {request.AssessmentId}");

        var employeeId = assessment.EmployeeId;

        // Step 3: Duplicate active plan check — BEFORE ML call
        var existingPlan = await _actionPlans.GetActiveByAssessmentIdAsync(request.AssessmentId);
        if (existingPlan is not null)
            throw new ArgumentException("Bu assessment için zaten aktif bir aksiyon planı mevcut.");

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

            // Step 11: Bulk load action catalog entries
            var codes   = recommendedActions.Select(x => x.dto.Code);
            var catalog = (await _actionCatalog.GetByCodesAsync(codes))
                .ToDictionary(c => c.Code, StringComparer.OrdinalIgnoreCase);

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

            // Step 13: Create action plan items
            foreach (var (action, rank) in recommendedActions)
            {
                var predicted = predictedActions.First(p => p.ActionCode == action.Code);
                catalog.TryGetValue(action.Code, out var entry);

                var item = new ActionPlanItem
                {
                    ActionPlanId        = plan.Id,
                    ActionCatalogId     = entry?.Id,
                    AiPredictedActionId = predicted.Id,
                    Title               = entry?.Title ?? $"Gelişim Aksiyonu: {action.Code}",
                    Description         = entry?.Description ?? "Bu aksiyon için lütfen yöneticinizle iletişime geçin.",
                    Priority            = entry?.DefaultPriority ?? PriorityLevel.Medium,
                    Source              = ActionPlanItemSource.AI,
                    OrderNo             = rank
                };
                await _actionPlans.AddItemAsync(item);
            }

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
        var plans = await _actionPlans.GetByEmployeeIdAsync(employeeId);
        var result = new List<ActionPlanDetailDto>();
        foreach (var p in plans)
        {
            var withItems = await _actionPlans.GetByIdWithItemsAsync(p.Id);
            if (withItems is not null)
                result.Add(ToDetailDto(withItems));
        }
        return result;
    }

    public Task<ActionPlanItemDto> UpdateActionPlanItemAsync(int planId, int itemId, UpdateActionPlanItemRequest request)
        => throw new NotImplementedException("Faz 11'de implement edilecek.");

    public Task<ActionPlanItemDto> AddManualItemAsync(int planId, AddManualActionPlanItemRequest request, int requestingUserId)
        => throw new NotImplementedException("Faz 11'de implement edilecek.");

    public Task RemoveItemAsync(int planId, int itemId)
        => throw new NotImplementedException("Faz 11'de implement edilecek.");

    public Task<ActionPlanDetailDto> ApproveActionPlanAsync(int id, int requestingUserId)
        => throw new NotImplementedException("Faz 11'de implement edilecek.");

    public Task<ActionPlanDetailDto> SendActionPlanToEmployeeAsync(int id, int requestingUserId)
        => throw new NotImplementedException("Faz 11'de implement edilecek.");

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static Dictionary<string, object> BuildFeatureDictionary(EmployeeFeatureDto f) => new()
    {
        ["Age"]                     = f.Age,
        ["Attrition"]               = f.Attrition,
        ["BusinessTravel"]          = f.BusinessTravel,
        ["Department"]              = f.Department,
        ["DistanceFromHome"]        = f.DistanceFromHome,
        ["Education"]               = f.Education,
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
            .OrderBy(i => i.OrderNo)
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
        Priority            = item.Priority.ToString(),
        DueDate             = item.DueDate,
        Source              = item.Source.ToString(),
        OrderNo             = item.OrderNo
    };
}
