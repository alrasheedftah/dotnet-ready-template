using Application.Shared.Messaging;
using Application.Shared.Persistence;
using Application.Shared.Pipeline;

namespace Application.Behaviors.UnitOfWork;

public sealed class UnitOfWorkBehavior<TCommand, TResponse>
    : ICommandPipelineBehavior<TCommand, TResponse>, IOrderedBehavior
    where TCommand : IBaseCommand
{
    private readonly IUnitOfWork _uow;
    public UnitOfWorkBehavior(
        IUnitOfWork uow)
        => _uow = uow;

    public int Order => BehaviorOrder.SaveChanges;

    public async Task<TResponse> Handle(TCommand command, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var response = await next(ct);
        await _uow.SaveChangesAsync(ct);
        return response;
    }
}
