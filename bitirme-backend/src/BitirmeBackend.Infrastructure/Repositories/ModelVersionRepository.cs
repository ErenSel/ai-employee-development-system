using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BitirmeBackend.Infrastructure.Repositories;

public class ModelVersionRepository : IModelVersionRepository
{
    private readonly AppDbContext _db;
    public ModelVersionRepository(AppDbContext db) => _db = db;

    public Task<ModelVersion?> GetActiveAsync() =>
        _db.ModelVersions.FirstOrDefaultAsync(m => m.IsActive && !m.IsDeleted);

    public Task<ModelVersion?> GetByIdAsync(int id) =>
        _db.ModelVersions.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

    public async Task<IEnumerable<ModelVersion>> GetAllAsync() =>
        await _db.ModelVersions.Where(m => !m.IsDeleted).ToListAsync();
}
