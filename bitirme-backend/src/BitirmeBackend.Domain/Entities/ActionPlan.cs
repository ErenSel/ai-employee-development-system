using BitirmeBackend.Domain.Common;
using BitirmeBackend.Domain.Enums;

namespace BitirmeBackend.Domain.Entities;

public class ActionPlan : BaseEntity
{
    public int AssessmentId { get; set; }
    public int EmployeeId { get; set; }
    public int CreatedByUserId { get; set; }
    public ActionPlanStatus Status { get; set; } = ActionPlanStatus.Draft;
    public DateTime? ApprovedAt { get; set; }
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// LLM-generated personalized evaluation text, produced once when the plan is created
    /// and reused by the PDF export. Null/empty means no summary (PDF skips the section).
    /// </summary>
    public string? AiSummary { get; set; }

    public Assessment Assessment { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
    public ICollection<ActionPlanItem> Items { get; set; } = [];
}
