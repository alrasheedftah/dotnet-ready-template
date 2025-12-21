using Domain.Shared;

namespace Application.Shared.Persistence;

public interface IDomainEventsAccessor
{
    IReadOnlyCollection<IDomainEvent> Drain();
}
