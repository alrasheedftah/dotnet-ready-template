using Application.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Outbox;

public sealed class EfOutboxReader : IOutboxReader
{
    private readonly AppDbContext _db;

    public EfOutboxReader(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<OutboxEnvelope>> GetUnpublishedAsync(int batchSize, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        // Simple polling. (Template version)
        // For multi-publisher safety, later you can add "LockUntilUtc/LockedBy" columns.
        var rows = await _db.OutboxMessages
            .Where(x => x.PublishedAtUtc == null &&
                        (x.NextAttemptAtUtc == null || x.NextAttemptAtUtc <= now))
            .OrderBy(x => x.OccurredAtUtc)
            .Take(batchSize)
            .ToListAsync(ct);

        return rows.Select(x => new OutboxEnvelope(x.Id, x.EventName, x.PayloadJson, x.OccurredAtUtc, x.Attempts)).ToList();
    }

    public async Task MarkPublishedAsync(Guid id, DateTime publishedAtUtc, CancellationToken ct)
    {
        var row = await _db.OutboxMessages.FirstAsync(x => x.Id == id, ct);
        row.PublishedAtUtc = publishedAtUtc;
        row.LastError = null;
        row.NextAttemptAtUtc = null;
        await _db.SaveChangesAsync(ct);
    }

    public async Task MarkFailedAsync(Guid id, string error, int attempts, DateTime? nextAttemptAtUtc, CancellationToken ct)
    {
        var row = await _db.OutboxMessages.FirstAsync(x => x.Id == id, ct);
        row.Attempts = attempts;
        row.LastError = error;
        row.NextAttemptAtUtc = nextAttemptAtUtc;
        await _db.SaveChangesAsync(ct);
    }
}
