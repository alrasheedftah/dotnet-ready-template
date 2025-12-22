namespace Application.Messaging.Messaging;


public sealed record ExternalMessage(
    string Name,
    string PayloadJson,
    Guid? MessageId = null,
    string? Key = null,
    IReadOnlyDictionary<string, string>? Headers = null
);

public interface IMessagePublisher
{
    Task PublishAsync(ExternalMessage message, CancellationToken ct);
}
