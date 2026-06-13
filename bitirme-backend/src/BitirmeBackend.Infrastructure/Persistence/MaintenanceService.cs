using BitirmeBackend.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BitirmeBackend.Infrastructure.Persistence;

/// <summary>
/// Resets runtime-generated (transactional) data while preserving every seed/reference table.
/// Deletes run in FK-safe order (children before parents) inside one transaction so the
/// operation is all-or-nothing.
/// </summary>
public class MaintenanceService : IMaintenanceService
{
    private readonly AppDbContext _db;
    private readonly ILogger<MaintenanceService> _logger;

    public MaintenanceService(AppDbContext db, ILogger<MaintenanceService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IReadOnlyDictionary<string, int>> ResetTransactionalDataAsync()
    {
        var counts = new Dictionary<string, int>();

        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            // FK-safe order: delete children before their parents. Reference/seed tables
            // (Users, Employees, Roles, Departments, JobRoles, Competencies, AssessmentCycles,
            // ActionCatalogs, ModelVersions, RefreshTokens) are intentionally not touched.
            counts["TaskComments"] = await _db.TaskComments.ExecuteDeleteAsync();
            counts["EmployeeTasks"] = await _db.EmployeeTasks.ExecuteDeleteAsync();
            counts["ActionPlanItems"] = await _db.ActionPlanItems.ExecuteDeleteAsync();
            counts["ActionPlans"] = await _db.ActionPlans.ExecuteDeleteAsync();
            counts["AiPredictedActions"] = await _db.AiPredictedActions.ExecuteDeleteAsync();
            counts["AiPredictionRuns"] = await _db.AiPredictionRuns.ExecuteDeleteAsync();
            counts["FeedbackComments"] = await _db.FeedbackComments.ExecuteDeleteAsync();
            counts["AssessmentScores"] = await _db.AssessmentScores.ExecuteDeleteAsync();
            counts["AssessmentAssignments"] = await _db.AssessmentAssignments.ExecuteDeleteAsync();
            counts["Assessments"] = await _db.Assessments.ExecuteDeleteAsync();

            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }

        var total = counts.Values.Sum();
        _logger.LogWarning("Transactional data reset by admin - {Total} rows deleted across {Tables} tables: {@Counts}",
            total, counts.Count, counts);

        return counts;
    }
}
