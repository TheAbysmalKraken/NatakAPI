using Catan.Core.Services;
using Catan.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace Catan.Infrastructure;

public class InMemoryActiveGameCache(IMemoryCache memoryCache) : IActiveGameCache
{
    public Task UpsetAsync(
        string gameId,
        Game game,
        TimeSpan? expiresIn = null,
        CancellationToken cancellationToken = default)
    {
        memoryCache.Set(gameId, game, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiresIn ?? TimeSpan.FromMinutes(15)
        });

        return Task.CompletedTask;
    }

    public Task<Game?> GetAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var game = memoryCache.Get<Game>(gameId);

        return Task.FromResult(game);
    }

    public Task RemoveAsync(string gameId, CancellationToken cancellationToken = default)
    {
        memoryCache.Remove(gameId);

        return Task.CompletedTask;
    }
}
