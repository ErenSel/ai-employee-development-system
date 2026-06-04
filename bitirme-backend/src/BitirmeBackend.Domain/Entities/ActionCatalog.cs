namespace BitirmeBackend.Domain.Entities;

/// <summary>
/// Action catalog backed by the ML model's action labels (see docs/db.sql).
/// Uses the AI action code (e.g. "DEPT_COMP1_03") as a string primary key so it
/// matches recommendedActions[].code coming back from the ML service one-to-one.
/// Does NOT inherit BaseEntity because the key is a string, not an int Id.
/// </summary>
public class ActionCatalog
{
    public string ActionId { get; set; } = null!;
    public string TargetCompetency { get; set; } = null!;
    public string ActionCategory { get; set; } = null!;
    public string ActionType { get; set; } = null!;
    public string Difficulty { get; set; } = null!;
    public int EstimatedEffortHours { get; set; }
    public decimal MinScore { get; set; }
    public decimal MaxScore { get; set; }

    /// <summary>Raw JSONB payload. Parse with System.Text.Json — never treat as plain text.</summary>
    public string ContentData { get; set; } = null!;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    public ICollection<ActionPlanItem> ActionPlanItems { get; set; } = [];
}
