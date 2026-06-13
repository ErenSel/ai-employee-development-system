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
    private readonly IUnitOfWork _uow;

    private const int RequiredCompetencyCount = 13;

    public AssessmentService(IAssessmentRepository assessments, IActionPlanRepository actionPlans, IUnitOfWork uow)
    {
        _assessments = assessments;
        _actionPlans = actionPlans;
        _uow = uow;
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
        if (await _assessments.HasActiveByEmployeeIdAsync(request.EmployeeId))
            throw new ArgumentException("Çalışanın devam eden aktif bir değerlendirme süreci bulunmaktadır.");

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
        await _uow.SaveChangesAsync();

        // FIX 4: auto-create the Self assignment so the employee can self-evaluate
        // without HR having to add it manually.
        await _assessments.AddAssignmentAsync(new AssessmentAssignment
        {
            AssessmentId        = assessment.Id,
            EvaluatorEmployeeId = assessment.EmployeeId,
            EvaluatorType       = "Self",
            IsCompleted         = false
        });
        await _uow.SaveChangesAsync();

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
        await _uow.SaveChangesAsync();

        var updated = await _assessments.GetByIdWithDetailsAsync(id)
            ?? throw new InvalidOperationException("Güncellenen değerlendirme yüklenemedi.");
        return ToDetail(updated);
    }

    public async Task<List<AssessmentScoreDto>> GetAssessmentScoresAsync(int assessmentId)
    {
        var scores = await _assessments.GetScoresByAssessmentIdAsync(assessmentId);
        return scores.Select(ToScoreDto).ToList();
    }

    public async Task<List<AssessmentScoreDto>> GetAssessmentScoresForEvaluatorAsync(int assessmentId, int evaluatorEmployeeId)
    {
        var scores = await _assessments.GetScoresByAssessmentIdAsync(assessmentId);
        return scores
            .Where(s => s.EvaluatorEmployeeId == evaluatorEmployeeId)
            .Select(ToScoreDto)
            .ToList();
    }

    public async Task<AssessmentScoreDto> UpsertAssessmentScoreAsync(int assessmentId, UpsertAssessmentScoreRequest request, bool allowProxy = false)
    {
        _ = await _assessments.GetByIdAsync(assessmentId)
            ?? throw new KeyNotFoundException($"Değerlendirme bulunamadı: {assessmentId}");

        if (request.Score < 0.0 || request.Score > 5.0)
            throw new ArgumentException($"Skor 0 ile 5 arasında olmalıdır. Girilen değer: {request.Score}");

        // 360°: scoring is gated by the evaluator's assignment, not the assessment status.
        // HR/Admin proxy ("God Mode") bypasses the assignment gate entirely so a score can be
        // written on anyone's behalf, even with no assignment or an already-completed survey.
        var assignment = await _assessments.GetAssignmentAsync(assessmentId, request.EvaluatorEmployeeId);
        if (!allowProxy)
        {
            if (assignment is null)
                throw new ArgumentException("Bu değerlendirme için atama bulunamadı.");
            if (assignment.IsCompleted)
                throw new ArgumentException("Bu anketi zaten tamamladınız.");
        }

        // Upsert keyed by (AssessmentId, CompetencyId, EvaluatorEmployeeId)
        var existing = await _assessments.GetScoreAsync(assessmentId, request.CompetencyId, request.EvaluatorEmployeeId);

        AssessmentScore savedCandidate;
        if (existing is not null)
        {
            existing.Score     = request.Score;
            existing.UpdatedAt = DateTime.UtcNow;
            _assessments.UpdateScore(existing);
            await _uow.SaveChangesAsync();
            savedCandidate = existing;
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
            await _uow.SaveChangesAsync();
            savedCandidate = score;
        }

        var allScores = (await _assessments.GetScoresByAssessmentIdAsync(assessmentId)).ToList();
        var saved = allScores.FirstOrDefault(s =>
                    s.CompetencyId == request.CompetencyId &&
                    s.EvaluatorEmployeeId == request.EvaluatorEmployeeId)
                ?? savedCandidate;

        // Mark the survey complete once the evaluator has scored all 13 competencies.
        // Only meaningful when an assignment exists (a pure HR/Admin proxy injection may have none).
        var scoredCompetencies = allScores
            .Where(s => s.EvaluatorEmployeeId == request.EvaluatorEmployeeId)
            .Select(s => s.CompetencyId)
            .Distinct()
            .Count();

        if (assignment is not null && !assignment.IsCompleted && scoredCompetencies >= RequiredCompetencyCount)
        {
            assignment.IsCompleted = true;
            assignment.CompletedAt = DateTime.UtcNow;
            _assessments.UpdateAssignment(assignment);

            // FIX 1: auto-complete the assessment once every assignment is done.
            await TryAutoCompleteAssessmentAsync(assessmentId);
            await _uow.SaveChangesAsync();
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

        // FIX 3: Self and Manager are unique per assessment; Peer/Subordinate may repeat.
        var isSelf    = string.Equals(request.EvaluatorType, "Self",    StringComparison.OrdinalIgnoreCase);
        var isManager = string.Equals(request.EvaluatorType, "Manager", StringComparison.OrdinalIgnoreCase);
        if (isSelf || isManager)
        {
            var assignments = await _assessments.GetAssignmentsByAssessmentAsync(request.AssessmentId);
            if (assignments.Any(a => string.Equals(a.EvaluatorType, request.EvaluatorType, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException(isSelf
                    ? "Bu değerlendirme için Self ataması zaten mevcut."
                    : "Bu değerlendirme için Manager ataması zaten mevcut.");
        }

        var assignment = new AssessmentAssignment
        {
            AssessmentId        = request.AssessmentId,
            EvaluatorEmployeeId = request.EvaluatorEmployeeId,
            EvaluatorType       = request.EvaluatorType,
            IsCompleted         = false
        };

        await _assessments.AddAssignmentAsync(assignment);
        await _uow.SaveChangesAsync();

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

    public async Task<bool> HasActiveAssignmentForEmployeeAsync(int employeeId, int evaluatorEmployeeId)
    {
        var assignments = await _assessments.GetAssignmentsByEvaluatorAsync(evaluatorEmployeeId);
        return assignments.Any(a =>
            !a.IsCompleted &&
            a.Assessment is not null &&
            a.Assessment.EmployeeId == employeeId &&
            a.Assessment.Status != AssessmentStatus.Completed);
    }

    public async Task<bool> HasAssignmentAsync(int assessmentId, int evaluatorEmployeeId)
    {
        var assignment = await _assessments.GetAssignmentAsync(assessmentId, evaluatorEmployeeId);
        return assignment is not null;
    }

    public async Task<List<AssessmentAssignmentDto>> GetAssignmentsByAssessmentAsync(int assessmentId)
    {
        var assignments = await _assessments.GetAssignmentsByAssessmentAsync(assessmentId);
        return assignments.Select(ToAssignmentDto).ToList();
    }

    public async Task<AssessmentAssignmentDto> BulkUpsertScoresAsync(int assessmentId, BulkUpsertAssessmentScoreRequest request, bool allowProxy = false)
    {
        _ = await _assessments.GetByIdAsync(assessmentId)
            ?? throw new KeyNotFoundException($"Değerlendirme bulunamadı: {assessmentId}");

        var assignment = await _assessments.GetAssignmentAsync(assessmentId, request.EvaluatorEmployeeId);

        if (assignment is null)
        {
            if (!allowProxy)
                throw new ArgumentException("Bu değerlendirme için atama bulunamadı.");

            // HR/Admin proxy ("God Mode"): no assignment yet — register one on the fly so the
            // scores stay tied to an assignment and the completion/auto-complete flow still works.
            if (!Enum.TryParse<EvaluatorType>(request.EvaluatorType, ignoreCase: true, out _))
                throw new ArgumentException("Ataması olmayan bir değerlendirici adına toplu skor girişinde geçerli bir EvaluatorType (Self/Manager/Peer/Subordinate) zorunludur.");

            assignment = new AssessmentAssignment
            {
                AssessmentId        = assessmentId,
                EvaluatorEmployeeId = request.EvaluatorEmployeeId,
                EvaluatorType       = request.EvaluatorType,
                IsCompleted         = false
            };
            await _assessments.AddAssignmentAsync(assignment);
            await _uow.SaveChangesAsync();
        }
        else if (assignment.IsCompleted && !allowProxy)
        {
            throw new ArgumentException("Bu anketi zaten tamamladınız.");
        }

        // Validate every score before persisting anything.
        foreach (var item in request.Scores)
        {
            if (item.Score < 0.0 || item.Score > 5.0)
                throw new ArgumentException($"Skor 0 ile 5 arasında olmalıdır. Girilen değer: {item.Score}");
        }

        if (!Enum.TryParse<EvaluatorType>(assignment.EvaluatorType, ignoreCase: true, out var evalType))
            throw new ArgumentException($"Geçersiz EvaluatorType: {assignment.EvaluatorType}");

        var existingScores = (await _assessments.GetScoresByAssessmentIdAsync(assessmentId))
            .Where(s => s.EvaluatorEmployeeId == request.EvaluatorEmployeeId)
            .ToDictionary(s => s.CompetencyId);

        // Upsert each score keyed by (AssessmentId, CompetencyId, EvaluatorEmployeeId).
        foreach (var item in request.Scores)
        {
            if (existingScores.TryGetValue(item.CompetencyId, out var existing))
            {
                existing.Score     = item.Score;
                existing.UpdatedAt = DateTime.UtcNow;
                _assessments.UpdateScore(existing);
            }
            else
            {
                await _assessments.AddScoreAsync(new AssessmentScore
                {
                    AssessmentId        = assessmentId,
                    CompetencyId        = item.CompetencyId,
                    EvaluatorEmployeeId = request.EvaluatorEmployeeId,
                    EvaluatorType       = evalType,
                    Score               = item.Score
                });
            }
        }
        await _uow.SaveChangesAsync();

        // Mark the survey complete once all 13 competencies are scored.
        var scoredCompetencies = (await _assessments.GetScoresByAssessmentIdAsync(assessmentId))
            .Where(s => s.EvaluatorEmployeeId == request.EvaluatorEmployeeId)
            .Select(s => s.CompetencyId)
            .Distinct()
            .Count();

        if (!assignment.IsCompleted && scoredCompetencies >= RequiredCompetencyCount)
        {
            assignment.IsCompleted = true;
            assignment.CompletedAt = DateTime.UtcNow;
            _assessments.UpdateAssignment(assignment);

            // FIX 1: auto-complete the assessment once every assignment is done.
            await TryAutoCompleteAssessmentAsync(assessmentId);
            await _uow.SaveChangesAsync();
        }

        var saved = await _assessments.GetAssignmentAsync(assessmentId, request.EvaluatorEmployeeId) ?? assignment;
        return ToAssignmentDto(saved);
    }

    // Sets the assessment to Completed once every one of its evaluator assignments is done.
    private async Task TryAutoCompleteAssessmentAsync(int assessmentId)
    {
        var assignments = await _assessments.GetAssignmentsByAssessmentAsync(assessmentId);
        if (assignments.Count == 0 || !assignments.All(a => a.IsCompleted))
            return;

        var assessment = await _assessments.GetByIdAsync(assessmentId);
        if (assessment is not null)
        {
            var scores = await _assessments.GetScoresByAssessmentIdAsync(assessmentId);
            assessment.Status       = AssessmentStatus.Completed;
            assessment.OverallScore = CalculateOverallScore(scores);
            assessment.UpdatedAt    = DateTime.UtcNow;
            _assessments.Update(assessment);
        }
    }

    private static double? CalculateOverallScore(IEnumerable<AssessmentScore> scores)
    {
        var competencyAverages = scores
            .GroupBy(s => s.CompetencyId)
            .Select(g => g.Average(s => s.Score))
            .ToList();

        return competencyAverages.Count == 0 ? null : competencyAverages.Average();
    }

    public async Task<PagedResponse<AssessmentDetailDto>> GetEmployeeAssessmentsAsync(
        int employeeId, int pageNumber, int pageSize)
    {
        var (items, total) = await _assessments.GetByEmployeePagedWithDetailsAsync(employeeId, pageNumber, pageSize);
        var dtos = items.Select(ToDetail).ToList();

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
