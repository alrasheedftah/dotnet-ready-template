using Application.Shared.Messaging;
using Application.Shared.Persistence;
using System.Text.Json;

namespace Infrastructure.Persistence.Outbox;

public sealed class EfOutbox : IOutbox
{
    private readonly AppDbContext _db;

    public EfOutbox(AppDbContext db) => _db = db;

    public Task AddAsync(IIntegrationEvent @event, CancellationToken ct)
    {
        var type = @event.EventName;

        var msg = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventName = type,
            PayloadJson = JsonSerializer.Serialize(@event, @event.GetType()),
            OccurredAtUtc = @event.OccurredAtUtc,
            CreatedAtUtc = DateTime.UtcNow,
            Attempts = 0
        };

        _db.OutboxMessages.Add(msg);
        return Task.CompletedTask;
    }
}
