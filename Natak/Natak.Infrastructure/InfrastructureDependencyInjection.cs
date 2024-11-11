using Microsoft.Extensions.Configuration;
using Natak.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Natak.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis");
        
        if (string.IsNullOrWhiteSpace(redisConnectionString))
        {
            services.AddInMemoryCache();
        }
        else
        {
            services.AddRedisCache(redisConnectionString);
        }
        
        return services;
    }
    
    private static IServiceCollection AddRedisCache(this IServiceCollection services, string redisConnectionString)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "Natak";
        });
        
        services.AddScoped<IActiveGameCache, RedisActiveGameCache>();

        return services;
    }
    
    private static IServiceCollection AddInMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        
        services.AddScoped<IActiveGameCache, InMemoryActiveGameCache>();

        return services;
    }
}
