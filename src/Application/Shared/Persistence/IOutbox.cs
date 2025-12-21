namespace Application.Shared.Persistence;

public interface IOutbox
{
    Task AddAsync(object integrationEvent, DateTime occurredAtUtc, CancellationToken ct);
}
