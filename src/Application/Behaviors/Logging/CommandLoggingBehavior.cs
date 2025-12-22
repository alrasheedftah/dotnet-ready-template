using System.Diagnostics;
using Application.Shared.Messaging;
using Application.Shared.Pipeline;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors.Logging;

public sealed class CommandLoggingBehavior<TCommand, TResponse>
    : ICommandPipelineBehavior<TCommand, TResponse>, IOrderedBehavior
    where TCommand : IBaseCommand
{

    private readonly ILogger<CommandLoggingBehavior<TCommand, TResponse>> _logger;

    public CommandLoggingBehavior(ILogger<CommandLoggingBehavior<TCommand, TResponse>> logger)
        => _logger = logger;

    public int Order => BehaviorOrder.Logging;

    public async Task<TResponse> Handle(TCommand command, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var commandName = typeof(TCommand).Name;

        // Optional: enable this only in dev, or when you have safe DTOs (avoid PII).
        // _logger.LogInformation("Handling command {CommandName}: {@Command}", commandName, command);

        _logger.LogInformation("Handling command {CommandName}", commandName);

        var sw = Stopwatch.StartNew();

        try
        {
            var response = await next(ct);
            sw.Stop();

            _logger.LogInformation(
                "Handled command {CommandName} in {ElapsedMs} ms",
                commandName,
                sw.ElapsedMilliseconds);

            return response;
        }catch(Exception ex)
        {
            sw.Stop();
            _logger.LogError(
                ex,
                "Command {CommandName} failed after {ElapsedMs} ms",
                commandName,
                sw.ElapsedMilliseconds);

            throw;
        }
    }
}