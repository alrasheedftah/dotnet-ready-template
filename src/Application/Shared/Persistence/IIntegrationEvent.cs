namespace Application.Shared.Messaging;

public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime OccurredAtUtc { get; }
    string EventName { get; } 
}

public abstract record IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
    public abstract string EventName { get; }
}