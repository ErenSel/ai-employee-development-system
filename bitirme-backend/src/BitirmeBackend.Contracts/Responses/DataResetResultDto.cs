namespace BitirmeBackend.Contracts.Responses;

/// <summary>
/// Result of an admin "reset transactional data" operation: how many rows were deleted
/// per table, and the total. Reference/seed tables are never touched.
/// </summary>
public class DataResetResultDto
{
    /// <summary>Deleted row count keyed by table name (for example, "Assessments" = 12).</summary>
    public Dictionary<string, int> DeletedCounts { get; set; } = new();

    public int TotalDeleted { get; set; }
    public DateTime ExecutedAt { get; set; }
}
