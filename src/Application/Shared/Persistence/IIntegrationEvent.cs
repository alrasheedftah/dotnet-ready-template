namespace Application.Shared.Messaging;

public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime OccurredAtUtc { get; }
    string Type { get; }
}
