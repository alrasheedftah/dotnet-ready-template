namespace Application.Shared.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct);
}
