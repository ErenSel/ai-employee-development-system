using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Application.Interfaces.Services;
using BitirmeBackend.Contracts.Common;
using BitirmeBackend.Contracts.Requests;
using BitirmeBackend.Contracts.Responses;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Domain.Enums;

namespace BitirmeBackend.Application.Services;

public class AssessmentService : IAssessmentService
{
    private readonly IAssessmentRepository _assessments;
    private readonly IActionPlanRepository _actionPlans;

    private const int RequiredCompetencyCount = 13;

    public AssessmentService(IAssessmentRepository assessments, IActionPlanRepository actionPlans)
    {
        _assessments = assessments;
        _actionPlans = actionPlans;
    }

    public async Task<AssessmentDetailDto> GetAssessmentByIdAsync(int id)
    {
        var a = await _assessments.GetByIdWithDetailsAsync(id)
            ?? throw new KeyNotFoundException($"Değerlendirme bulunamadı: {id}");
        return ToDetail(a);
    }

    public async Task<AssessmentDetailDto> CreateAssessmentAsync(CreateAssessmentRequest request, int createdByUserId)
    {
        // Active incomplete plan guard — block a new assessment while the employee
        // still has a development plan that is not Completed/Cancelled.
        var plans = await _actionPlans.GetByEmployeeIdAsync(request.EmployeeId);
        var activePlan = plans.FirstOrDefault(p =>
            p.Status != ActionPlanStatus.Completed && p.Status != ActionPlanStatus.Cancelled);
        if (activePlan is not null)
            throw new ArgumentException("Çalışanın devam eden tamamlanmamış bir gelişim planı bulunmaktadır. Yeni bir değerlendirme süreci başlatılamaz.");

        var assessment = new Assessment
        {
            EmployeeId      = request.EmployeeId,
            CycleId         = request.CycleId,
            Status          = AssessmentStatus.Draft,
            CreatedByUserId = createdByUserId
        };

        await _assessments.AddAsync(assessment);

        var created = await _assessments.GetByIdWithDetailsAsync(assessment.Id)
            ?? throw new InvalidOperationException("Oluşturulan değerlendirme yüklenemedi.");
        return ToDetail(created);
    }

    public async Task<AssessmentDetailDto> CompleteAssessmentAsync(int id)
    {
        var a = await _assessments.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Değerlendirme bulunamadı: {id}");

        if (a.Status != AssessmentStatus.Draft)
            throw new ArgumentException($"Yalnızca Draft durumundaki değerlendirmeler tamamlanabilir. Mevcut durum: {a.Status}");

        a.Status    = AssessmentStatus.Completed;
        a.UpdatedAt = DateTime.UtcNow;
        _assessments.Update(a);

        var updated = await _assessments.GetByIdWithDetailsAsync(id)
            ?? throw new InvalidOperationException("Güncellenen değerlendirme yüklenemedi.");
        return ToDetail(updated);
    }

    public async Task<List<AssessmentScoreDto>> GetAssessmentScoresAsync(int assessmentId)
    {
        var scores = await _assessments.GetScoresByAssessmentIdAsync(assessmentId);
        return scores.Select(ToScoreDto).ToList();
    }

    public async Task<AssessmentScoreDto> UpsertAssessmentScoreAsync(int assessmentId, UpsertAssessmentScoreRequest request)
    {
        _ = await _assessments.GetByIdAsync(assessmentId)
            ?? throw new KeyNotFoundException($"Değerlendirme bulunamadı: {assessmentId}");

        if (request.Score < 0.0 || request.Score > 5.0)
            throw new ArgumentException($"Skor 0 ile 5 arasında olmalıdır. Girilen değer: {request.Score}");

        // 360°: scoring is gated by the evaluator's assignment, not the assessment status.
        var assignment = await _assessments.GetAssignmentAsync(assessmentId, request.EvaluatorEmployeeId)
            ?? throw new ArgumentException("Bu değerlendirme için atama bulunamadı.");

        if (assignment.IsCompleted)
            throw new ArgumentException("Bu anketi zaten tamamladınız.");

        // Upsert keyed by (AssessmentId, CompetencyId, EvaluatorEmployeeId)
        var existing = await _assessments.GetScoreAsync(assessmentId, request.CompetencyId, request.EvaluatorEmployeeId);

        AssessmentScore saved;
        if (existing is not null)
        {
            existing.Score     = request.Score;
            existing.UpdatedAt = DateTime.UtcNow;
            _assessments.UpdateScore(existing);

            var allScores = await _assessments.GetScoresByAssessmentIdAsync(assessmentId);
            saved = allScores.FirstOrDefault(s => s.Id == existing.Id) ?? existing;
        }
        else
        {
            if (!Enum.TryParse<EvaluatorType>(request.EvaluatorType, ignoreCase: true, out var evalType))
                throw new ArgumentException($"Geçersiz EvaluatorType: {request.EvaluatorType}");

            var score = new AssessmentScore
            {
                AssessmentId        = assessmentId,
                CompetencyId        = request.CompetencyId,
                EvaluatorEmployeeId = request.EvaluatorEmployeeId,
                EvaluatorType       = evalType,
                Score               = request.Score
            };

            await _assessments.AddScoreAsync(score);

            var scores = await _assessments.GetScoresByAssessmentIdAsync(assessmentId);
            saved = scores.FirstOrDefault(s => s.Id == score.Id) ?? score;
        }

        // Mark the survey complete once the evaluator has scored all 13 competencies.
        var scoredCompetencies = (await _assessments.GetScoresByAssessmentIdAsync(assessmentId))
            .Where(s => s.EvaluatorEmployeeId == request.EvaluatorEmployeeId)
            .Select(s => s.CompetencyId)
            .Distinct()
            .Count();

        if (scoredCompetencies >= RequiredCompetencyCount)
        {
            assignment.IsCompleted = true;
            assignment.CompletedAt = DateTime.UtcNow;
            _assessments.UpdateAssignment(assignment);
        }

        return ToScoreDto(saved);
    }

    public async Task<AssessmentAssignmentDto> CreateAssignmentAsync(CreateAssessmentAssignmentRequest request, int requestingUserId)
    {
        _ = await _assessments.GetByIdAsync(request.AssessmentId)
            ?? throw new KeyNotFoundException($"Değerlendirme bulunamadı: {request.AssessmentId}");

        var existing = await _assessments.GetAssignmentAsync(request.AssessmentId, request.EvaluatorEmployeeId);
        if (existing is not null)
            throw new ArgumentException("Bu değerlendirici zaten atanmış.");

        var assignment = new AssessmentAssignment
        {
            AssessmentId        = request.AssessmentId,
            EvaluatorEmployeeId = request.EvaluatorEmployeeId,
            EvaluatorType       = request.EvaluatorType,
            IsCompleted         = false
        };

        await _assessments.AddAssignmentAsync(assignment);

        // Reload with evaluator nav attached
        var saved = await _assessments.GetAssignmentAsync(request.AssessmentId, request.EvaluatorEmployeeId) ?? assignment;
        return ToAssignmentDto(saved);
    }

    public async Task<List<MySurveyDto>> GetMySurveysAsync(int evaluatorEmployeeId)
    {
        var assignments = await _assessments.GetAssignmentsByEvaluatorAsync(evaluatorEmployeeId);

        return assignments
            .Where(a => !a.IsCompleted
                && a.Assessment is not null
                && (a.Assessment.Status == AssessmentStatus.Draft || a.Assessment.Status == AssessmentStatus.Completed))
            .Select(a => new MySurveyDto
            {
                AssignmentId    = a.Id,
                AssessmentId    = a.AssessmentId,
                EmployeeId      = a.Assessment.EmployeeId,
                EmployeeName    = a.Assessment.Employee?.FullName ?? string.Empty,
                CycleId         = a.Assessment.CycleId,
                CycleName       = a.Assessment.Cycle?.Name ?? string.Empty,
                EvaluatorType   = a.EvaluatorType,
                CompetencyCount = RequiredCompetencyCount
            })
            .ToList();
    }

    public async Task<List<AssessmentAssignmentDto>> GetAssignmentsByAssessmentAsync(int assessmentId)
    {
        var assignments = await _assessments.GetAssignmentsByAssessmentAsync(assessmentId);
        return assignments.Select(ToAssignmentDto).ToList();
    }

    public async Task<PagedResponse<AssessmentDetailDto>> GetEmployeeAssessmentsAsync(
        int employeeId, int pageNumber, int pageSize)
    {
        var (items, total) = await _assessments.GetByEmployeePagedAsync(employeeId, pageNumber, pageSize);

        // Load details for each (attach nav properties)
        var dtos = new List<AssessmentDetailDto>();
        foreach (var a in items)
        {
            var detailed = await _assessments.GetByIdWithDetailsAsync(a.Id);
            if (detailed is not null) dtos.Add(ToDetail(detailed));
        }

        return PagedResponse<AssessmentDetailDto>.Ok(dtos, total, pageNumber, pageSize);
    }

    // ── Mapping helpers ──────────────────────────────────────────────────────

    private static AssessmentDetailDto ToDetail(Assessment a) => new()
    {
        Id                = a.Id,
        EmployeeId        = a.EmployeeId,
        EmployeeName      = a.Employee?.FullName ?? string.Empty,
        CycleId           = a.CycleId,
        CycleName         = a.Cycle?.Name ?? string.Empty,
        OverallScore      = a.OverallScore,
        Status            = a.Status.ToString(),
        CreatedByUserId   = a.CreatedByUserId,
        CreatedByUserName = a.CreatedByUser?.FullName ?? string.Empty,
        CreatedAt         = a.CreatedAt,
        UpdatedAt         = a.UpdatedAt
    };

    private static AssessmentScoreDto ToScoreDto(AssessmentScore s) => new()
    {
        Id                  = s.Id,
        AssessmentId        = s.AssessmentId,
        CompetencyId        = s.CompetencyId,
        CompetencyCode      = s.Competency?.Code ?? string.Empty,
        CompetencyName      = s.Competency?.Name ?? string.Empty,
        EvaluatorEmployeeId = s.EvaluatorEmployeeId,
        EvaluatorType       = s.EvaluatorType.ToString(),
        Score               = s.Score
    };

    private static AssessmentAssignmentDto ToAssignmentDto(AssessmentAssignment a) => new()
    {
        Id                    = a.Id,
        AssessmentId          = a.AssessmentId,
        EvaluatorEmployeeId   = a.EvaluatorEmployeeId,
        EvaluatorEmployeeName = a.EvaluatorEmployee?.FullName ?? string.Empty,
        EvaluatorType         = a.EvaluatorType,
        IsCompleted           = a.IsCompleted,
        CompletedAt           = a.CompletedAt
    };
}
