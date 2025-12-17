using Application.Shared.Messaging;

namespace Application.Shared.Pipeline;

public interface ICommandPipelineBehavior<TCommand, TResponse>
    where TCommand : IBaseCommand
{
    Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next, CancellationToken ct);
}



