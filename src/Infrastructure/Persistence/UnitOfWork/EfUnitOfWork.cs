using Application.Shared.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence.UnitOfWork;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private IDbContextTransaction? _tx;

    public EfUnitOfWork(AppDbContext db) => _db = db;

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken ct)
    {
        var tx = await _db.Database.BeginTransactionAsync(ct);
        return new EfTransaction(tx);
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);

    // public async Task CommitAsync(CancellationToken ct)
    // {
    //     if (_tx is null) return;
    //     await _tx.CommitAsync(ct);
    //     await _tx.DisposeAsync();
    //     _tx = null;
    // }

    // public async Task RollbackAsync(CancellationToken ct)
    // {
    //     if (_tx is null) return;
    //     await _tx.RollbackAsync(ct);
    //     await _tx.DisposeAsync();
    //     _tx = null;
    // }
}
