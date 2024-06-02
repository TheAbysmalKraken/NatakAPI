using Catan.Application.Models;
using Catan.Domain;
using Catan.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace Catan.Application;

public sealed class GameManager(IMemoryCache cache) : IGameManager
{
    public Result EmbargoPlayer(string gameId, int playerColour, int playerColourToEmbargo)
    {
        var gameResult = GetGameFromCache(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (!IsValidPlayerColour(playerColourToEmbargo, game.PlayerCount)
        || !IsValidPlayerColour(playerColour, game.PlayerCount))
        {
            return Result.Failure(Errors.InvalidPlayerColour);
        }

        if (game.GameSubPhase != GameSubPhase.TradeOrBuild)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        var embargoSuccess = game.EmbargoPlayer((PlayerColour)playerColour, (PlayerColour)playerColourToEmbargo);

        if (!embargoSuccess)
        {
            return Result.Failure(Errors.CannotEmbargoPlayer);
        }

        SetGameInCache(game);

        return Result.Success();
    }

    private static bool IsValidPlayerColour(int colour, int playerCount)
    {
        return colour >= 0 && colour < playerCount;
    }

    private Result<Game> GetGameFromCache(string gameId)
    {
        var game = cache.Get<Game>(gameId);

        if (game is null)
        {
            return Result.Failure<Game>(Errors.GameNotFound);
        }

        return Result.Success(game);
    }

    private void SetGameInCache(Game game)
    {
        cache.Set(game.Id, game, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
        });
    }
}
