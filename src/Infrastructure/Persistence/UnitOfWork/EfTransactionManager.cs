using Application.Shared.Persistence;

namespace Infrastructure.Persistence.UnitOfWork;

public sealed class EfTransactionManager : ITransactionManager
{
    private readonly AppDbContext _db;
    public EfTransactionManager(AppDbContext db) => _db = db;
    public async Task<ITransaction> BeginTransactionAsync(CancellationToken ct = default)
    {
        var tx = await _db.Database.BeginTransactionAsync(ct);
        return new EfTransaction(tx);
    }
}
