namespace Application.Shared.Persistence;

public interface IUnitOfWork
{
    Task<ITransaction> BeginTransactionAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
