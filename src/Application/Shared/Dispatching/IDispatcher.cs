using Application.Shared.Messaging;

namespace Application.Shared.Dispatching;

public interface IDispatcher
{
    Task<TResponse> Send<TCommand, TResponse>(TCommand command, CancellationToken ct = default)
        where TCommand : ICommand<TResponse>;

    Task<TResponse> Query<TQuery, TResponse>(TQuery query, CancellationToken ct = default)
        where TQuery : IQuery<TResponse>;
}