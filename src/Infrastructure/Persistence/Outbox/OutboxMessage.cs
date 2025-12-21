namespace Infrastructure.Persistence.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;
    public string PayloadJson { get; set; } = default!;
    public DateTime OccurredAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime? PublishedAtUtc { get; set; }

    public int Attempts { get; set; }
    public DateTime? NextAttemptAtUtc { get; set; }
    public string? LastError { get; set; }
}
