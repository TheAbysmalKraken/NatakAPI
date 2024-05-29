using Catan.Application.Models;
using Catan.Domain;
using Catan.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace Catan.Application;

public sealed class GameManager(IMemoryCache cache) : IGameManager
{
    public Result BuyDevelopmentCard(string gameId)
    {
        var gameResult = GetGameFromCache(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GameSubPhase != GameSubPhase.PlayTurn
        && game.GameSubPhase != GameSubPhase.TradeOrBuild)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        var buySuccess = game.BuyDevelopmentCard();

        if (!buySuccess)
        {
            return Result.Failure(Errors.CannotBuyDevelopmentCard);
        }

        SetGameInCache(game);

        return Result.Success();
    }

    public Result PlayKnightCard(string gameId, int x, int y, int playerColourToStealFrom)
    {
        var gameResult = GetGameFromCache(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (!IsValidPlayerColour(playerColourToStealFrom, game.PlayerCount))
        {
            return Result.Failure<PlayerSpecificGameStatusResponse>(Errors.InvalidPlayerColour);
        }

        if (game.GameSubPhase != GameSubPhase.PlayTurn
        && game.GameSubPhase != GameSubPhase.TradeOrBuild
        && game.GameSubPhase != GameSubPhase.RollOrPlayDevelopmentCard)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        if (game.HasPlayedDevelopmentCardThisTurn)
        {
            return Result.Failure(Errors.AlreadyPlayedDevelopmentCard);
        }

        var playSuccess = game.PlayKnightCard(new(x, y), (PlayerColour)playerColourToStealFrom);

        if (!playSuccess)
        {
            return Result.Failure(Errors.CannotPlayDevelopmentCard);
        }

        SetGameInCache(game);

        return Result.Success();
    }

    public Result PlayRoadBuildingCard(
        string gameId, int firstX, int firstY, int secondX, int secondY,
        int thirdX, int thirdY, int fourthX, int fourthY)
    {
        var gameResult = GetGameFromCache(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GameSubPhase != GameSubPhase.PlayTurn
        && game.GameSubPhase != GameSubPhase.TradeOrBuild
        && game.GameSubPhase != GameSubPhase.RollOrPlayDevelopmentCard)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        if (game.HasPlayedDevelopmentCardThisTurn)
        {
            return Result.Failure(Errors.AlreadyPlayedDevelopmentCard);
        }

        var playSuccess = game.PlayRoadBuildingCard(new(firstX, firstY), new(secondX, secondY), new(thirdX, thirdY), new(fourthX, fourthY));

        if (!playSuccess)
        {
            return Result.Failure(Errors.CannotPlayDevelopmentCard);
        }

        SetGameInCache(game);

        return Result.Success();
    }

    public Result PlayYearOfPlentyCard(string gameId, int resourceType1, int resourceType2)
    {
        var gameResult = GetGameFromCache(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GameSubPhase != GameSubPhase.PlayTurn
        && game.GameSubPhase != GameSubPhase.TradeOrBuild
        && game.GameSubPhase != GameSubPhase.RollOrPlayDevelopmentCard)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        if (game.HasPlayedDevelopmentCardThisTurn)
        {
            return Result.Failure(Errors.AlreadyPlayedDevelopmentCard);
        }

        var playSuccess = game.PlayYearOfPlentyCard((ResourceType)resourceType1, (ResourceType)resourceType2);

        if (!playSuccess)
        {
            return Result.Failure(Errors.CannotPlayDevelopmentCard);
        }

        SetGameInCache(game);

        return Result.Success();
    }

    public Result PlayMonopolyCard(string gameId, int resourceType)
    {
        var gameResult = GetGameFromCache(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GameSubPhase != GameSubPhase.PlayTurn
        && game.GameSubPhase != GameSubPhase.TradeOrBuild
        && game.GameSubPhase != GameSubPhase.RollOrPlayDevelopmentCard)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        if (game.HasPlayedDevelopmentCardThisTurn)
        {
            return Result.Failure(Errors.AlreadyPlayedDevelopmentCard);
        }

        var playSuccess = game.PlayMonopolyCard((ResourceType)resourceType);

        if (!playSuccess)
        {
            return Result.Failure(Errors.CannotPlayDevelopmentCard);
        }

        SetGameInCache(game);

        return Result.Success();
    }

    public Result MoveRobber(string gameId, int x, int y)
    {
        var gameResult = GetGameFromCache(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GameSubPhase != GameSubPhase.MoveRobberSevenRoll
        && game.GameSubPhase != GameSubPhase.MoveRobberKnightCardBeforeRoll
        && game.GameSubPhase != GameSubPhase.MoveRobberKnightCardAfterRoll)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        var moveSuccess = game.MoveRobber(new(x, y));

        if (!moveSuccess)
        {
            return Result.Failure(Errors.CannotMoveRobberToLocation);
        }

        SetGameInCache(game);

        return Result.Success();
    }

    public Result StealResource(string gameId, int victimColour)
    {
        var gameResult = GetGameFromCache(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (!IsValidPlayerColour(victimColour, game.PlayerCount))
        {
            return Result.Failure(Errors.InvalidPlayerColour);
        }

        if (game.GameSubPhase != GameSubPhase.StealResourceSevenRoll
        && game.GameSubPhase != GameSubPhase.StealResourceKnightCardBeforeRoll
        && game.GameSubPhase != GameSubPhase.StealResourceKnightCardAfterRoll)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        var stealSuccess = game.StealResourceCard((PlayerColour)victimColour);

        if (!stealSuccess)
        {
            return Result.Failure(Errors.CannotStealResource);
        }

        SetGameInCache(game);

        return Result.Success();
    }

    public Result DiscardResources(string gameId, int playerColour, Dictionary<int, int> resources)
    {
        var gameResult = GetGameFromCache(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GameSubPhase != GameSubPhase.DiscardResources)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        var catanResources = resources.ToDictionary(kvp => (ResourceType)kvp.Key, kvp => kvp.Value);

        var discardSuccess = game.DiscardResources((PlayerColour)playerColour, catanResources);

        if (!discardSuccess)
        {
            return Result.Failure(Errors.CannotDiscardResources);
        }

        game.TryFinishDiscardingResources();
        SetGameInCache(game);

        return Result.Success();
    }

    public Result TradeWithBank(string gameId, int resourceToGive, int resourceToGet)
    {
        var gameResult = GetGameFromCache(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GameSubPhase != GameSubPhase.TradeOrBuild)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        var tradeTwoToOneSuccess = game.TradeTwoToOne((ResourceType)resourceToGive, (ResourceType)resourceToGet);

        if (tradeTwoToOneSuccess)
        {
            SetGameInCache(game);
            return Result.Success();
        }

        var tradeThreeToOneSuccess = game.TradeThreeToOne((ResourceType)resourceToGive, (ResourceType)resourceToGet);

        if (tradeThreeToOneSuccess)
        {
            SetGameInCache(game);
            return Result.Success();
        }

        var tradeFourToOneSuccess = game.TradeFourToOne((ResourceType)resourceToGive, (ResourceType)resourceToGet);

        if (tradeFourToOneSuccess)
        {
            SetGameInCache(game);
            return Result.Success();
        }

        return Result.Failure(Errors.CannotTradeWithBank);
    }

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
