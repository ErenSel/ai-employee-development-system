using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;
using BitirmeBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BitirmeBackend.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _db;
    public RefreshTokenRepository(AppDbContext db) => _db = db;

    /// <summary>Caller passes the already-hashed token (raw tokens never reach the DB).</summary>
    public Task<RefreshToken?> GetByTokenHashAsync(string tokenHash) =>
        _db.RefreshTokens.FirstOrDefaultAsync(t =>
            t.TokenHash == tokenHash &&
            !t.IsRevoked &&
            t.ExpiresAt > DateTime.UtcNow);

    public async Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(int userId) =>
        await _db.RefreshTokens
            .Where(t => t.UserId == userId && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();

    public async Task AddAsync(RefreshToken token) => await _db.RefreshTokens.AddAsync(token);

    public void Update(RefreshToken token) { /* tracked entity; SaveChanges persists changes */ }
}
