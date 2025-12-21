namespace Application.Shared.Persistence;

public interface IOutboxReader
{
    Task<IReadOnlyList<OutboxEnvelope>> GetUnpublishedAsync(int batchSize, CancellationToken ct);
    Task MarkPublishedAsync(Guid id, DateTime publishedAtUtc, CancellationToken ct);
    Task MarkFailedAsync(Guid id, string error, int attempts, DateTime? nextAttemptAtUtc, CancellationToken ct);
}

public sealed record OutboxEnvelope(
    Guid Id,
    string Type,
    string PayloadJson,
    DateTime OccurredAtUtc,
    int Attempts
);
