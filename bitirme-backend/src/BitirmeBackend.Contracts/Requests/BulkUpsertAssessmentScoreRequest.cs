namespace BitirmeBackend.Contracts.Requests;

/// <summary>
/// Submits all of one evaluator's competency scores for an assessment in a single call.
/// </summary>
public class BulkUpsertAssessmentScoreRequest
{
    public int EvaluatorEmployeeId { get; set; }
    public List<ScoreItem> Scores { get; set; } = [];

    public class ScoreItem
    {
        public int CompetencyId { get; set; }
        public double Score { get; set; }
    }
}
