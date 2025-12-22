using Application.Behaviors.Logging;
using Application.Behaviors.UnitOfWork;
using Application.Shared.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Behaviors are cross-cutting "use-case pipeline" concerns.
        // Most common order for commands:
        // Validation
        // Logging
        // UnitOfWork + Outbox (transaction boundary)
        //
        // Because your dispatcher does Reverse(), register in "outer -> inner" order.
        // Outer should be registered first.
        services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(CommandLoggingBehavior<,>));
        services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));


        // COMMAND pipeline behaviors
        services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(CommandLoggingBehavior<,>));
        services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(OutboxBehavior<,>));
        services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        // QUERY pipeline behaviors
        services.AddScoped(typeof(IQueryPipelineBehavior<,>), typeof(QueryLoggingBehavior<,>));

        // If you later add ValidationBehavior:
        // services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(CommandValidationBehavior<,>));

        return services;
    }
}
