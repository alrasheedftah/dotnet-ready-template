using Application.Shared.Messaging;

namespace Application.Shared.Persistence;

public interface IOutbox
{
    Task AddAsync(IIntegrationEvent @event, CancellationToken ct);
}
