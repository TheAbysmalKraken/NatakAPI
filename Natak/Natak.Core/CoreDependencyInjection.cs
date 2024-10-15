using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Natak.Core.Abstractions.Behaviours;

namespace Natak.Core;

public static class CoreDependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
        });

        return services;
    }
}
