using BitirmeBackend.Contracts.Common;
using BitirmeBackend.Contracts.Requests;
using BitirmeBackend.Contracts.Responses;

namespace BitirmeBackend.Application.Interfaces.Services;

public interface IAssessmentService
{
    Task<AssessmentDetailDto> GetAssessmentByIdAsync(int id);
    Task<AssessmentDetailDto> CreateAssessmentAsync(CreateAssessmentRequest request, int createdByUserId);
    Task<AssessmentDetailDto> CompleteAssessmentAsync(int id);
    Task<List<AssessmentScoreDto>> GetAssessmentScoresAsync(int assessmentId);
    Task<List<AssessmentScoreDto>> GetAssessmentScoresForEvaluatorAsync(int assessmentId, int evaluatorEmployeeId);
    /// <param name="allowProxy">
    /// When true (HR/Admin "God Mode"), the evaluator-assignment requirement is bypassed so a
    /// score can be written on anyone's behalf without an existing/incomplete assignment.
    /// </param>
    Task<AssessmentScoreDto> UpsertAssessmentScoreAsync(int assessmentId, UpsertAssessmentScoreRequest request, bool allowProxy = false);
    Task<PagedResponse<AssessmentDetailDto>> GetEmployeeAssessmentsAsync(int employeeId, int pageNumber, int pageSize);
    Task<bool> HasActiveAssignmentForEmployeeAsync(int employeeId, int evaluatorEmployeeId);
    Task<bool> HasAssignmentAsync(int assessmentId, int evaluatorEmployeeId);

    // ── 360° evaluator assignments ─────────────────────────────────────────
    Task<AssessmentAssignmentDto> CreateAssignmentAsync(CreateAssessmentAssignmentRequest request, int requestingUserId);
    Task<List<MySurveyDto>> GetMySurveysAsync(int evaluatorEmployeeId);
    Task<List<AssessmentAssignmentDto>> GetAssignmentsByAssessmentAsync(int assessmentId);

    /// <summary>Submits all of one evaluator's competency scores at once; completes the survey when all 13 are scored.</summary>
    /// <param name="allowProxy">
    /// When true (HR/Admin "God Mode"), the assignment requirement is bypassed: a missing
    /// assignment is created on the fly (using <see cref="BulkUpsertAssessmentScoreRequest.EvaluatorType"/>)
    /// and an already-completed survey may be corrected.
    /// </param>
    Task<AssessmentAssignmentDto> BulkUpsertScoresAsync(int assessmentId, BulkUpsertAssessmentScoreRequest request, bool allowProxy = false);
}
