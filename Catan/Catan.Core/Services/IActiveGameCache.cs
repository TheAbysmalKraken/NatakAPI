using Catan.Domain;

namespace Catan.Core.Services;

public interface IActiveGameCache
{
    Task UpsetAsync(
        string gameId,
        Game game,
        TimeSpan? expiresIn = null,
        CancellationToken cancellationToken = default);

    Task<Game?> GetAsync(string gameId, CancellationToken cancellationToken = default);

    Task RemoveAsync(string gameId, CancellationToken cancellationToken = default);
}
