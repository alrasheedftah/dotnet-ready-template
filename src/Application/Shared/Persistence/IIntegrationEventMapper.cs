using Domain.Shared;

namespace Application.Shared.Messaging;

public interface IIntegrationEventMapper
{
    IEnumerable<IIntegrationEvent> Map(IDomainEvent domainEvent);
}
