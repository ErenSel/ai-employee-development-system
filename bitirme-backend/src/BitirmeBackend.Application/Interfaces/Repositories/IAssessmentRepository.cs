using BitirmeBackend.Domain.Entities;

namespace BitirmeBackend.Application.Interfaces.Repositories;

public interface IAssessmentRepository
{
    Task<Assessment?> GetByIdAsync(int id);
    Task<Assessment?> GetByIdWithDetailsAsync(int id);
    Task<(IEnumerable<Assessment> Items, int TotalCount)> GetByEmployeePagedAsync(int employeeId, int pageNumber, int pageSize);
    Task<(IEnumerable<Assessment> Items, int TotalCount)> GetByEmployeePagedWithDetailsAsync(int employeeId, int pageNumber, int pageSize);
    Task<IEnumerable<AssessmentScore>> GetScoresByAssessmentIdAsync(int assessmentId);
    Task<AssessmentScore?> GetScoreAsync(int assessmentId, int competencyId, string evaluatorType);

    /// <summary>Looks up a score by its 360° unique key (Assessment, Competency, Evaluator).</summary>
    Task<AssessmentScore?> GetScoreAsync(int assessmentId, int competencyId, int evaluatorEmployeeId);

    Task AddAsync(Assessment assessment);
    Task AddScoreAsync(AssessmentScore score);
    void Update(Assessment assessment);
    void UpdateScore(AssessmentScore score);

    // ── 360° evaluator assignments ─────────────────────────────────────────
    Task<AssessmentAssignment?> GetAssignmentAsync(int assessmentId, int evaluatorEmployeeId);
    Task AddAssignmentAsync(AssessmentAssignment assignment);
    Task<List<AssessmentAssignment>> GetAssignmentsByEvaluatorAsync(int evaluatorEmployeeId);
    Task<List<AssessmentAssignment>> GetAssignmentsByAssessmentAsync(int assessmentId);
    void UpdateAssignment(AssessmentAssignment assignment);
}
