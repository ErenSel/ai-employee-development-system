using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BitirmeBackend.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _db;
    public RoleRepository(AppDbContext db) => _db = db;

    public Task<Role?> GetByIdAsync(int id) =>
        _db.Roles.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

    public Task<Role?> GetByNameAsync(string name) =>
        _db.Roles.FirstOrDefaultAsync(r => r.Name == name && !r.IsDeleted);

    public async Task<IEnumerable<Role>> GetAllAsync() =>
        await _db.Roles.Where(r => !r.IsDeleted).ToListAsync();
}
