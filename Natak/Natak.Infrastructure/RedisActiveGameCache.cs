using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Natak.Core.Services;
using Natak.Domain;

namespace Natak.Infrastructure;

public sealed class RedisActiveGameCache(IDistributedCache distributedCache) : IActiveGameCache
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        IncludeFields = true
    };
    
    public async Task UpsetAsync(
        string gameId,
        Game game,
        TimeSpan? expiresIn = null,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(game, JsonSerializerOptions);
        
        await distributedCache.SetStringAsync(gameId, json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiresIn ?? TimeSpan.FromMinutes(15)
        }, cancellationToken);
    }

    public async Task<Game?> GetAsync(
        string gameId,
        CancellationToken cancellationToken = default)
    {
        var json = await distributedCache.GetStringAsync(gameId, cancellationToken);
        
        return json is null
            ? null
            : JsonSerializer.Deserialize<Game>(json, JsonSerializerOptions);
    }

    public async Task RemoveAsync(
        string gameId,
        CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(gameId, cancellationToken);
    }
}