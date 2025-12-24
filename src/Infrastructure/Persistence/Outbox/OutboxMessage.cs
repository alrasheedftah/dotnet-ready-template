namespace Infrastructure.Persistence.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public string EventName { get; set; } = default!;
    public string PayloadJson { get; set; } = default!;
    public string? HeadersJson { get; set; }
    public DateTime OccurredAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime? PublishedAtUtc { get; set; }

    public int Attempts { get; set; }
    public DateTime? NextAttemptAtUtc { get; set; }
    public string? LastError { get; set; }

    // for scale-out safety
    public string? LockedBy { get; set; }
    public DateTime? LockedUntilUtc { get; set; }    
}
