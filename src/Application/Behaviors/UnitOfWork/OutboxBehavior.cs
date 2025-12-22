using Application.Shared.Messaging;
using Application.Shared.Persistence;
using Application.Shared.Pipeline;

namespace Application.Behaviors.UnitOfWork;

public sealed class OutboxBehavior<TCommand, TResponse>
    : ICommandPipelineBehavior<TCommand, TResponse>, IOrderedBehavior
    where TCommand : IBaseCommand
{
    private readonly IDomainEventsAccessor _events;
    private readonly IIntegrationEventMapper _mapper;
    private readonly IOutbox _outbox;

    public OutboxBehavior(
        IDomainEventsAccessor events,
        IIntegrationEventMapper mapper,
        IOutbox outbox)
    {
        _events = events;
        _mapper = mapper;
        _outbox = outbox;
    }

    public int Order => BehaviorOrder.Outbox;

    public async Task<TResponse> Handle(
        TCommand command,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var response = await next(ct);

        var domainEvents = _events.Drain();

        // only update the changetracker 
        // save will be done in uow behavior last ordering in pipeline
        foreach (var de in domainEvents)
        foreach (var ie in _mapper.Map(de))
            await _outbox.AddAsync(ie, ct);

        return response;
    }
}
