using BitirmeBackend.Domain.Common;

namespace BitirmeBackend.Domain.Entities;

/// <summary>
/// A 360° evaluator assignment: links one evaluator (employee) to an assessment with a
/// given evaluator role. One survey per (Assessment, Evaluator) — enforced by a unique index.
/// </summary>
public class AssessmentAssignment : BaseEntity
{
    public int AssessmentId { get; set; }
    public int EvaluatorEmployeeId { get; set; }

    /// <summary>Self / Manager / Peer / Subordinate (stored as string in the DB).</summary>
    public string EvaluatorType { get; set; } = string.Empty;

    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedAt { get; set; }

    public Assessment Assessment { get; set; } = null!;
    public Employee? EvaluatorEmployee { get; set; }
}
