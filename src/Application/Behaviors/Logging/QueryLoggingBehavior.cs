using System.Diagnostics;
using Application.Shared.Messaging;
using Application.Shared.Pipeline;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors.Logging;

public sealed class QueryLoggingBehavior<TQuery, TResponse> : IQueryPipelineBehavior<TQuery, TResponse>, IOrderedBehavior
    where TQuery : IQuery<TResponse>
{

    private ILogger<QueryLoggingBehavior<TQuery, TResponse>> _logger;

    public QueryLoggingBehavior(ILogger<QueryLoggingBehavior<TQuery, TResponse>> logger)
        => _logger = logger;

    public int Order => BehaviorOrder.Logging;

    public async Task<TResponse> Handle(TQuery query, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var queryName = typeof(TQuery).Name;

        _logger.LogDebug("Handling query {QueryName}", queryName);
        var sw = Stopwatch.StartNew();
        try
        {
            var response = await next(ct);
            sw.Stop();
            _logger.LogInformation(
                "Handled Query {QueryName} in {ElapsedMs} ms",
                queryName,
                sw.ElapsedMilliseconds);
            
            return response;
        }catch(Exception ex)
        {
            sw.Stop();
            _logger.LogError(
                ex,
                "Query {QueryName} failed after {ElapsedMs} ms",
                queryName,
                sw.ElapsedMilliseconds);
            throw;
        }
    }
}