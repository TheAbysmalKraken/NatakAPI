using Catan.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Catan.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IActiveGameCache, InMemoryActiveGameCache>();

        return services;
    }
}
