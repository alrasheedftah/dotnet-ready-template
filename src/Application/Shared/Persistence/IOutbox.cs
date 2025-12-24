using Application.Messaging.Messaging;

namespace Application.Shared.Persistence;

public interface IOutbox
{
    Task AddAsync(ExternalMessage @event, CancellationToken ct);
}
