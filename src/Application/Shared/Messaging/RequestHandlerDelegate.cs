namespace Application.Shared.Messaging;

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>(CancellationToken ct);
