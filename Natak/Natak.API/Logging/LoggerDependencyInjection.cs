using Serilog;
using Serilog.Events;

namespace Natak.API.Logging;

public static class LoggerDependencyInjection
{
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt")
            .CreateLogger();

        services.AddSerilog(logger);

        return services;
    }
}