namespace BitirmeBackend.Contracts.Responses;

/// <summary>
/// A pending survey the calling employee must fill in as a 360-degree evaluator.
/// </summary>
public class MySurveyDto
{
    public int AssignmentId { get; set; }
    public int AssessmentId { get; set; }
    public int EvaluatorEmployeeId { get; set; }

    /// <summary>The employee being evaluated (the subject of the assessment).</summary>
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int CycleId { get; set; }
    public string CycleName { get; set; } = string.Empty;
    public string EvaluatorType { get; set; } = string.Empty;

    /// <summary>Number of competencies to score - always 13.</summary>
    public int CompetencyCount { get; set; } = 13;
}
