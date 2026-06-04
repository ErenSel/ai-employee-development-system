using BitirmeBackend.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BitirmeBackend.Infrastructure.Persistence;

/// <summary>
/// EF Core unit of work. Shares the request-scoped <see cref="AppDbContext"/> with the
/// repositories so SaveChanges and transactions span every repository call in the request.
/// Commit/Rollback are tolerant of being called with no active transaction (the action
/// plan generation flow can commit a failed-run record and then hit the outer rollback).
/// </summary>
public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private IDbContextTransaction? _transaction;

    public EfUnitOfWork(AppDbContext db) => _db = db;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _db.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            return; // already in a transaction — reuse it

        _transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose() => _transaction?.Dispose();
}
