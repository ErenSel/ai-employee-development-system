using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BitirmeBackend.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public Task<User?> GetByIdAsync(int id) =>
        _db.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

    public Task<User?> GetByEmailAsync(string email) =>
        _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted);

    public Task<User?> GetByIdWithRoleAsync(int id) =>
        _db.Users.Include(u => u.Role)
                 .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

    public Task<bool> ExistsByEmailAsync(string email) =>
        _db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted);

    public async Task AddAsync(User user) => await _db.Users.AddAsync(user);

    public void Update(User user) => user.UpdatedAt = DateTime.UtcNow;
}
