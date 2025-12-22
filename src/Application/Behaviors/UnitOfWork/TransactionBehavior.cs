
using Application.Shared.Messaging;
using Application.Shared.Persistence;
using Application.Shared.Pipeline;

namespace Application.Behaviors.UnitOfWork;

public sealed class TransactionBehavior<TCommand, TResponse>
    : ICommandPipelineBehavior<TCommand, TResponse>, IOrderedBehavior
    where TCommand : IBaseCommand
{
    private readonly ITransactionManager _transactionManager;

    public TransactionBehavior(ITransactionManager transactionManager)
        => _transactionManager = transactionManager;

    public int Order => BehaviorOrder.Transaction;

    public async Task<TResponse> Handle(
        TCommand command,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync(ct);

        try
        {
            var response = await next(ct);
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
