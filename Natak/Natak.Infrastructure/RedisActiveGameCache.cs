using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Infrastructure.Converters;
using Natak.Infrastructure.DTOs;

namespace Natak.Infrastructure;

public sealed class RedisActiveGameCache(IDistributedCache distributedCache) : IActiveGameCache
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(60);
    
    private static JsonSerializerOptions JsonSerializerOptions => new()
    {
        WriteIndented = true,
        Converters = { new StateManagerDtoConverter() }
    };
    
    public async Task UpsetAsync(
        string gameId,
        Game game,
        TimeSpan? expiresIn = null,
        CancellationToken cancellationToken = default)
    {
        var dto = GameDto.FromDomain(game);
        
        var json = JsonSerializer.Serialize(dto, JsonSerializerOptions);
        
        await distributedCache.SetStringAsync(gameId, json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiresIn ?? DefaultExpiration
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
        
        var dto = JsonSerializer.Deserialize<GameDto>(json, JsonSerializerOptions);

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