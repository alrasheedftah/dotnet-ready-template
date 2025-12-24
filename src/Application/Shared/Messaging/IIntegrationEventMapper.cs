using Application.Messaging.Messaging;
using Domain.Shared;

namespace Application.Shared.Messaging;

public interface IIntegrationEventMapper
{
    IReadOnlyCollection<ExternalMessage> Map(IDomainEvent domainEvent);
}
