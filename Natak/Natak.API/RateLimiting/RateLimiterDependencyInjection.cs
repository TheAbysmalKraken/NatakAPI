using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Natak.API.RateLimiting;

public static class RateLimiterDependencyInjection
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions
                .SetOnRejected()
                .AddDefaultRateLimiting()
                .AddGameCreationRateLimiting();
        });
        
        return services;
    }
    
    private static RateLimiterOptions SetOnRejected(this RateLimiterOptions rateLimiterOptions)
    {
        rateLimiterOptions.OnRejected = (context, _) =>
        {
            if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            {
                context.HttpContext.Response.Headers.RetryAfter =
                    ((int) retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
            }
            else
            {
                /* ASPNETCORE has a bug where the RetryAfter lease metadata
                 is not set for sliding window rate limiters, so we set the default
                 retry-after header to 5 seconds.
                 https://github.com/dotnet/aspnetcore/issues/52411 */
                context.HttpContext.Response.Headers.RetryAfter =
                    ((int) TimeSpan.FromSeconds(5).TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
            }

            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

            return ValueTask.CompletedTask;
        };
        
        return rateLimiterOptions;
    }
    
    private static RateLimiterOptions AddDefaultRateLimiting(this RateLimiterOptions rateLimiterOptions)
    {
        rateLimiterOptions.AddPolicy(RateLimiterConstants.DefaultPolicyName, httpContext =>
            RateLimitPartition.GetSlidingWindowLimiter(
                partitionKey: GetRemoteIpAddress(httpContext),
                factory: _ => new SlidingWindowRateLimiterOptions()
                {
                    PermitLimit = 20,
                    Window = TimeSpan.FromSeconds(5),
                    SegmentsPerWindow = 5,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                }));
        
        return rateLimiterOptions;
    }
    
    private static RateLimiterOptions AddGameCreationRateLimiting(this RateLimiterOptions rateLimiterOptions)
    {
        rateLimiterOptions.AddPolicy(RateLimiterConstants.CreateGamePolicyName, httpContext =>
            RateLimitPartition.GetTokenBucketLimiter(
                partitionKey: GetRemoteIpAddress(httpContext),
                factory: _ => new TokenBucketRateLimiterOptions()
                {
                    TokenLimit = 10,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0,
                    ReplenishmentPeriod = TimeSpan.FromHours(1),
                    TokensPerPeriod = 1,
                    AutoReplenishment = true
                }));
        
        return rateLimiterOptions;
    }
    
    private static string? GetRemoteIpAddress(this HttpContext httpContext)
    {
        var ipAddress = httpContext.Request.Headers["X-Forwarded-For"].ToString();
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
        }

        return ipAddress;
    }
}