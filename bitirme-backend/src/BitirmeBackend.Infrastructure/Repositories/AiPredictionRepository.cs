using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BitirmeBackend.Infrastructure.Repositories;

public class AiPredictionRepository : IAiPredictionRepository
{
    private readonly AppDbContext _db;
    public AiPredictionRepository(AppDbContext db) => _db = db;

    public Task<AiPredictionRun?> GetByIdAsync(int id) =>
        _db.AiPredictionRuns.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

    public Task<AiPredictionRun?> GetByIdWithActionsAsync(int id) =>
        _db.AiPredictionRuns
            .Include(r => r.PredictedActions.Where(a => !a.IsDeleted))
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

    public async Task AddRunAsync(AiPredictionRun run) => await _db.AiPredictionRuns.AddAsync(run);

    public async Task AddPredictedActionsAsync(IEnumerable<AiPredictedAction> actions) =>
        await _db.AiPredictedActions.AddRangeAsync(actions);

    public void UpdateRun(AiPredictionRun run) => run.UpdatedAt = DateTime.UtcNow;
}
