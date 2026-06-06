namespace BitirmeBackend.Contracts.Requests;

public class CreateAssessmentAssignmentRequest
{
    public int AssessmentId { get; set; }
    public int EvaluatorEmployeeId { get; set; }
    public string EvaluatorType { get; set; } = string.Empty;
}
