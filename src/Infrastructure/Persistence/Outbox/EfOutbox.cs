using Application.Messaging.Messaging;
using Application.Shared.Persistence;
using System.Text.Json;

namespace Infrastructure.Persistence.Outbox;

public sealed class EfOutbox : IOutbox
{
    private readonly AppDbContext _db;

    public EfOutbox(AppDbContext db) => _db = db;

    public Task AddAsync(ExternalMessage message, CancellationToken ct)
    {
        var msg = new OutboxMessage
        {
            Id = message.MessageId ?? Guid.NewGuid(),
            EventName = message.Name,
            PayloadJson = message.PayloadJson,
            HeadersJson = message.Headers is null
                ? null
                : JsonSerializer.Serialize(message.Headers),
            OccurredAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow,
            Attempts = 0
        };

        _db.OutboxMessages.Add(msg);
        return Task.CompletedTask;
    }
}
