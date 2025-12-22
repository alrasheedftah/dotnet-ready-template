using Application.Shared.Persistence;
using Domain.Shared;

namespace Infrastructure.Persistence.DomainEvents;

public sealed class EfDomainEventsAccessor : IDomainEventsAccessor
{
    private readonly AppDbContext _db;

    public EfDomainEventsAccessor(AppDbContext db) => _db = db;

    public IReadOnlyCollection<IDomainEvent> Drain()
    {
        // Find tracked aggregate roots
        var aggregates = _db.ChangeTracker
            .Entries()
            .Where(e => e.Entity is IHaveDomainEvents)
            .Select(e => (IHaveDomainEvents)e.Entity)
            .ToList();

        var events = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        foreach (var a in aggregates)
            a.ClearDomainEvents();

        return events;
    }

    // Small adapter interface so we can detect aggregates generically
    public interface IHaveDomainEvents
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
