using Application.Shared.Messaging;
using Application.Shared.Persistence;
using Application.Shared.Pipeline;

namespace Application.Behaviors.UnitOfWork;

public sealed class UnitOfWorkOutboxBehavior<TCommand, TResponse>
    : ICommandPipelineBehavior<TCommand, TResponse>
    where TCommand : IBaseCommand
{
    private readonly IUnitOfWork _uow;
    private readonly IDomainEventsAccessor _events;
    private readonly IIntegrationEventMapper _mapper;
    private readonly IOutbox _outbox;

    public UnitOfWorkOutboxBehavior(
        IUnitOfWork uow,
        IDomainEventsAccessor events,
        IIntegrationEventMapper mapper,
        IOutbox outbox)
    {
        _uow = uow;
        _events = events;
        _mapper = mapper;
        _outbox = outbox;
    }

    public async Task<TResponse> Handle(TCommand command, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        await using var transaction = await _uow.BeginTransactionAsync(ct);

        try
        {
            // Run handler
            var response = await next(ct);

            // Persist domain state (creates IDs, etc.)  will see it later
            // await _uow.SaveChangesAsync(ct);

            // Drain domain events from tracked aggregates
            var domainEvents = _events.Drain();

            // Map domain events -> integration events and store to Outbox
            foreach (var de in domainEvents)
                foreach (var ie in _mapper.Map(de))
                    await _outbox.AddAsync(ie, de.OccurredAtUtc, ct);

            // Persist outbox rows
            await _uow.SaveChangesAsync(ct);

            // Commit transaction
            await transaction.CommitAsync(ct);
            return response;
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}
