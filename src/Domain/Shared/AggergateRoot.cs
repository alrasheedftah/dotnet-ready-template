namespace Domain.Shared;

public abstract class AggregateRoot<TId> : Entity<TId>, IHaveDomainEvents
{
    private readonly List<IDomainEvent> _events = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();
    protected void Raise(IDomainEvent @event) => _events.Add(@event);
    public void ClearDomainEvents() => _events.Clear();
}
