using Application.Shared.Messaging;

namespace Application.Shared.Pipeline;

public interface IQueryPipelineBehavior<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<TResponse> Handle(TQuery request, RequestHandlerDelegate<TResponse> next, CancellationToken ct);
}