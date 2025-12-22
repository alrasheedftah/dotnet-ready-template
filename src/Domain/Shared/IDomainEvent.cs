namespace Domain.Shared;

public interface IDomainEvent
{
    DateTime OccurredAtUtc { get; }
}

public abstract record DomainEvent : IDomainEvent
{
    // Todo 
    // Domain must NOT call DateTime.UtcNow directly
    // domain may receive it from outside
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}

public interface IHaveDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}