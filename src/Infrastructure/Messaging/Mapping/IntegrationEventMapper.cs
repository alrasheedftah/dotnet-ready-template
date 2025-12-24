using Application.Messaging.Messaging;
using Application.Shared.Messaging;
using Domain.Shared;

namespace Infrastructure.Messaging.Mapping;

public sealed class IntegrationEventMapper : IIntegrationEventMapper
{
    private readonly IServiceProvider _sp;

    public IntegrationEventMapper(IServiceProvider sp) => _sp = sp;

    public IReadOnlyCollection<ExternalMessage> Map(IDomainEvent domainEvent)
    {
        // Resolve typed mapper: IDomainEventToExternalMessageMapper<TDomainEvent>
        var mapperType = typeof(IDomainEventToExternalMessageMapper<>).MakeGenericType(domainEvent.GetType());
        var mapper = _sp.GetService(mapperType);

        if (mapper is null)
            return Array.Empty<ExternalMessage>();

        // Call Map(...) without big switch
        return ((dynamic)mapper).Map((dynamic)domainEvent);
    }
}
