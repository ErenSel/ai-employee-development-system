namespace BitirmeBackend.Contracts.Responses;

public class EmployeeTaskDto
{
    public int Id { get; set; }
    public int ActionPlanItemId { get; set; }

    /// <summary>The development plan this task belongs to — lets the employee download the plan PDF.</summary>
    public int ActionPlanId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    /// <summary>Learning resource link/reference snapshotted from the action plan item.</summary>
    public string? Resource { get; set; }

    /// <summary>Delivery type of the action (e.g. course, mentoring) snapshotted from the item.</summary>
    public string? DeliveryType { get; set; }
    public string Priority { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int AssignedByUserId { get; set; }
    public string AssignedByUserName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
}
