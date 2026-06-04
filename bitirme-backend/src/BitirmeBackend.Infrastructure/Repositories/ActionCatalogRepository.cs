using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BitirmeBackend.Infrastructure.Repositories;

public class ActionCatalogRepository : IActionCatalogRepository
{
    private readonly AppDbContext _db;
    public ActionCatalogRepository(AppDbContext db) => _db = db;

    public Task<ActionCatalog?> GetByIdAsync(string actionId) =>
        _db.ActionCatalogs.FirstOrDefaultAsync(c => c.ActionId == actionId && !c.IsDeleted);

    public Task<ActionCatalog?> GetByCodeAsync(string code) =>
        _db.ActionCatalogs.FirstOrDefaultAsync(c => c.ActionId == code && !c.IsDeleted);

    public async Task<IEnumerable<ActionCatalog>> GetByCodesAsync(IEnumerable<string> codes)
    {
        var upper = codes.Select(c => c.ToUpper()).ToHashSet();
        return await _db.ActionCatalogs
            .Where(c => !c.IsDeleted && upper.Contains(c.ActionId.ToUpper()))
            .ToListAsync();
    }

    public async Task<IEnumerable<ActionCatalog>> GetAllActiveAsync() =>
        await _db.ActionCatalogs.Where(c => !c.IsDeleted && c.IsActive).ToListAsync();
}
