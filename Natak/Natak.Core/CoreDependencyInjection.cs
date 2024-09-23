using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Natak.Core;

public static class CoreDependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddSingleton<IMemoryCache, MemoryCache>();

        return services;
    }
}
