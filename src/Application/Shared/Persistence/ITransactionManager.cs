namespace Application.Shared.Persistence;

public interface ITransactionManager
{
    Task<ITransaction> BeginTransactionAsync(CancellationToken ct);
}