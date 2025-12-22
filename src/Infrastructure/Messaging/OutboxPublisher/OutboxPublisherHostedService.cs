using Application.Messaging.Messaging;
using Application.Shared.Persistence;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.OutboxPublisher;

public sealed class OutboxPublisherHostedService : BackgroundService
{
    private readonly IOutboxReader _outbox;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<OutboxPublisherHostedService> _logger;

    public OutboxPublisherHostedService(
        IOutboxReader outbox,
        IMessagePublisher publisher,
        ILogger<OutboxPublisherHostedService> logger)
    {
        _outbox = outbox;
        _publisher = publisher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int batchSize = 100;
        var delay = TimeSpan.FromSeconds(2);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var batch = await _outbox.GetUnpublishedAsync(batchSize, stoppingToken);

                foreach (var row in batch)
                {
                    try
                    {
                        var msg = new ExternalMessage(
                            Name: row.Name,
                            PayloadJson: row.PayloadJson,
                            MessageId: row.Id,
                            Key: row.Key,
                            Headers: row.Headers
                        );                        
                        await _publisher.PublishAsync(msg, stoppingToken);
                        await _outbox.MarkPublishedAsync(row.Id, DateTime.UtcNow, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        var attempts = row.Attempts + 1;

                        // simple backoff
                        var next = DateTime.UtcNow.AddSeconds(Math.Min(60, attempts * 5));

                        _logger.LogError(ex, "Outbox publish failed for {OutboxId} type {Type}", row.Id, row.Name);

                        // Don’t store raw ex.Message only
                        // Prefer a short error + maybe truncate stack trace in DB.
                        await _outbox.MarkFailedAsync(row.Id, ex.Message, attempts, next, stoppingToken);
                    }
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox publisher loop failed");
            }
            await Task.Delay(delay, stoppingToken);
        }
    }
}
