namespace BitirmeBackend.Contracts.Responses;

public class AssessmentAssignmentDto
{
    public int Id { get; set; }
    public int AssessmentId { get; set; }
    public int EvaluatorEmployeeId { get; set; }
    public string EvaluatorEmployeeName { get; set; } = string.Empty;
    public string EvaluatorType { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
