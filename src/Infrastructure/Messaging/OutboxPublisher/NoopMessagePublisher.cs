using Application.Messaging.Messaging;

namespace Infrastructure.Messaging.OutboxPublisher;

public sealed class NoopMessagePublisher : IMessagePublisher
{
    public Task PublishAsync(ExternalMessage message, CancellationToken ct) => Task.CompletedTask;
}
