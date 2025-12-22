using Application.Shared.Messaging;

namespace Application.Commands;

public sealed record PingCommand(string Message) : ICommand<string>;

public sealed class PingCommandHandler : ICommandHandler<PingCommand, string>
{
    public Task<string> Handle(PingCommand command, CancellationToken ct)
    {
        return Task.FromResult($"PONG: {command.Message}");
    }
}
