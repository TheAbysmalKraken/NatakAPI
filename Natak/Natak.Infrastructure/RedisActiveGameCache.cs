using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Infrastructure.DTOs;

namespace Natak.Infrastructure;

public sealed class RedisActiveGameCache(IDistributedCache distributedCache) : IActiveGameCache
{
    public async Task UpsetAsync(
        string gameId,
        Game game,
        TimeSpan? expiresIn = null,
        CancellationToken cancellationToken = default)
    {
        var dto = GameDto.FromDomain(game);
        
        var json = JsonSerializer.Serialize(dto);
        
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
        
        if (json is null)
        {
            return null;
        }
        
        var dto = JsonSerializer.Deserialize<GameDto>(json);

        if (dto is null)
        {
            throw new InvalidOperationException("Failed to deserialize game from cache.");
        }
        
        return dto.ToDomain();
    }

    public async Task RemoveAsync(
        string gameId,
        CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(gameId, cancellationToken);
    }
}