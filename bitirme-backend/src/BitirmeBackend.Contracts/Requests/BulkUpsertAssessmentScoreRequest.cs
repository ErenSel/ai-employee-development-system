namespace BitirmeBackend.Contracts.Requests;

/// <summary>
/// Submits all of one evaluator's competency scores for an assessment in a single call.
/// </summary>
public class BulkUpsertAssessmentScoreRequest
{
    public int EvaluatorEmployeeId { get; set; }

    /// <summary>
    /// Evaluator role (Self/Manager/Peer/Subordinate). Optional for the normal flow —
    /// the existing assignment supplies it. Required only when an HR/Admin proxy submits
    /// scores for an evaluator that has no assignment yet.
    /// </summary>
    public string EvaluatorType { get; set; } = string.Empty;

    public List<ScoreItem> Scores { get; set; } = [];

    public class ScoreItem
    {
        public int CompetencyId { get; set; }
        public double Score { get; set; }
    }
}
