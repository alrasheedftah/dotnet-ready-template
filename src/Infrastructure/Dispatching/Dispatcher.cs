using Application.Shared.Dispatching;
using Application.Shared.Messaging;
using Application.Shared.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Dispatching;

public sealed class Dispatcher : IDispatcher
{
    private readonly IServiceScopeFactory _scopeFactory;

    public Dispatcher(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

    public async Task<TResponse> Send<TCommand, TResponse>(TCommand command, CancellationToken ct = default)
        where TCommand : ICommand<TResponse>
    {
        using var scope = _scopeFactory.CreateScope();
        var sp = scope.ServiceProvider;

        var handler = sp.GetRequiredService<ICommandHandler<TCommand, TResponse>>();
        RequestHandlerDelegate<TResponse> next = innerCt => handler.Handle(command, innerCt);

        foreach (var behavior in sp.GetServices<ICommandPipelineBehavior<TCommand, TResponse>>().Reverse())
        {
            var current = next;
            next = innerCt => behavior.Handle(command, current, innerCt);
        }

        return await next(ct);
    }

    public async Task<TResponse> Query<TQuery, TResponse>(TQuery query, CancellationToken ct = default)
        where TQuery : IQuery<TResponse>
    {
        using var scope = _scopeFactory.CreateScope();
        var sp = scope.ServiceProvider;

        var handler = sp.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
        RequestHandlerDelegate<TResponse> next = innerCt => handler.Handle(query, innerCt);

        foreach (var behavior in sp.GetServices<IQueryPipelineBehavior<TQuery, TResponse>>().Reverse())
        {
            var current = next;
            next = innerCt => behavior.Handle(query, current, innerCt);
        }

        return await next(ct);
    }
}
