using Application.Messaging.Messaging;
using Application.Shared.Dispatching;
using Application.Shared.Persistence;
using Infrastructure.Dispatching;
using Infrastructure.Messaging.OutboxPublisher;
using Infrastructure.Persistence;
using Infrastructure.Persistence.DomainEvents;
using Infrastructure.Persistence.Outbox;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        // DbContext
        // var connStr = config.GetConnectionString("DefaultConnection")
        //               ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'.");

        // services.AddDbContext<AppDbContext>(opt =>
        // {
        //     // opt.UseSqlServer(connStr);
        //     // Optional: useful in dev
        //     // opt.EnableSensitiveDataLogging();
        //     // opt.EnableDetailedErrors();
        // });

        // Persistence abstractions implementations
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IDomainEventsAccessor, EfDomainEventsAccessor>();

        // Outbox storage (write) & reader (publisher)
        services.AddScoped<IOutbox, EfOutbox>();
        services.AddScoped<IOutboxReader, EfOutboxReader>();

        // External message publishing adapter
        // Template default: Noop (does nothing).
        // Replace with Kafka/Rabbit/AzureServiceBus later.
        services.AddSingleton<IMessagePublisher, NoopMessagePublisher>();

        // Outbox background publisher (Hosted Service)
        services.AddHostedService<OutboxPublisherHostedService>();

        // Custom mediator replacement
        services.AddScoped<IDispatcher, Dispatcher>();        

        return services;
    }
}
