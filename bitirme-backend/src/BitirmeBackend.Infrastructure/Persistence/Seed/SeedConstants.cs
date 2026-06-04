namespace BitirmeBackend.Infrastructure.Persistence.Seed;

/// <summary>
/// Shared constants for deterministic seed data. CreatedAt must be a fixed value
/// (not DateTime.UtcNow) so EF migrations stay stable between builds.
/// </summary>
public static class SeedConstants
{
    public static readonly DateTime SeedDate = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}
