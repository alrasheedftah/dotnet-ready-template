using Application.Messaging.Messaging;
using Domain.Shared;

namespace Application.Shared.Messaging;

public interface IDomainEventToExternalMessageMapper<in TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    IReadOnlyCollection<ExternalMessage> Map(TDomainEvent domainEvent);
}
