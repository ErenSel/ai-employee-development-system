namespace BitirmeBackend.Application.Interfaces.Services;

/// <summary>
/// Admin-only maintenance operations for resetting the system to a clean state.
/// Only runtime-generated (transactional) data is removed; seed/reference
/// tables (Users, Employees, Roles, Departments, JobRoles, Competencies, AssessmentCycles,
/// ActionCatalogs, ModelVersions, RefreshTokens) are preserved.
/// </summary>
public interface IMaintenanceService
{
    /// <summary>
    /// Physically deletes every assessment-, prediction-, plan- and task-related row in a
    /// single transaction (FK-safe order) and returns the deleted row count per table.
    /// </summary>
    Task<IReadOnlyDictionary<string, int>> ResetTransactionalDataAsync();
}
