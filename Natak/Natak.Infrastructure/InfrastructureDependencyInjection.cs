using Natak.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Natak.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddRedisCache();
        
        return services;
    }
    
    private static IServiceCollection AddInMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        
        services.AddScoped<IActiveGameCache, InMemoryActiveGameCache>();

        return services;
    }
    
    private static IServiceCollection AddRedisCache(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "Natak";
        });
        
        services.AddScoped<IActiveGameCache, RedisActiveGameCache>();

        return services;
    }
}
