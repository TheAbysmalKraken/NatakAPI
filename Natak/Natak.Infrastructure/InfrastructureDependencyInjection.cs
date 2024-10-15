using Microsoft.Extensions.Configuration;
using Natak.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Natak.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRedisCache(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis")
            ?? throw new InvalidOperationException("Redis connection string not found.");
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "Natak";
        });
        
        services.AddScoped<IActiveGameCache, RedisActiveGameCache>();

        return services;
    }
}
