using BitirmeBackend.Domain.Common;
using BitirmeBackend.Domain.Enums;

namespace BitirmeBackend.Domain.Entities;

public class AssessmentScore : BaseEntity
{
    public int AssessmentId { get; set; }
    public int CompetencyId { get; set; }

    /// <summary>The employee who produced this score (the evaluator).</summary>
    public int EvaluatorEmployeeId { get; set; }

    /// <summary>Still kept for grouping during 360° consolidation (Self/Manager/Peer/Subordinate).</summary>
    public EvaluatorType EvaluatorType { get; set; }
    public double Score { get; set; }

    public Assessment Assessment { get; set; } = null!;
    public Competency Competency { get; set; } = null!;
    public Employee? EvaluatorEmployee { get; set; }
}
