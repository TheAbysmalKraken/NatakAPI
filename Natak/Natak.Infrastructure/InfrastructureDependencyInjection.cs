using Natak.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Natak.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IActiveGameCache, InMemoryActiveGameCache>();

        return services;
    }
}
