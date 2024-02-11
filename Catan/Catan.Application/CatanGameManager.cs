using Catan.Application.Models;
using Catan.Domain;
using static Catan.Common.Enumerations;

namespace Catan.Application;

public sealed class CatanGameManager : ICatanGameManager
{
    private readonly List<CatanGame> currentGames = [];

    public Result<PlayerSpecificGameStatusResponse> GetGameStatus(string gameId, int playerColour)
    {
        var gameResult = GetGame(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure<PlayerSpecificGameStatusResponse>(gameResult.Error);
        }

        var game = gameResult.Value;

        if (!IsValidPlayerColour(playerColour))
        {
            return Result.Failure<PlayerSpecificGameStatusResponse>(CatanErrors.InvalidPlayerColour);
        }

        var response = PlayerSpecificGameStatusResponse.FromDomain(game, playerColour);

        return Result.Success(response);
    }

    public Result<List<CoordinatesResponse>> GetAvailableSettlementLocations(string gameId, int playerColour)
    {
        throw new NotImplementedException();
    }

    public Result<List<CoordinatesResponse>> GetAvailableCityLocations(string gameId, int playerColour)
    {
        throw new NotImplementedException();
    }

    public Result<List<RoadCoordinatesResponse>> GetAvailableRoadLocations(string gameId, int playerColour)
    {
        throw new NotImplementedException();
    }

    public Result<string> CreateNewGame(int playerCount)
    {
        if (playerCount < 3 || playerCount > 4)
        {
            return Result.Failure<string>(CatanErrors.InvalidPlayerCount);
        }

        var newGame = new CatanGame(playerCount);
        currentGames.Add(newGame);

        return Result.Success(newGame.Id);
    }

    public Result<List<int>> RollDice(string gameId)
    {
        var gameResult = GetGame(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure<List<int>>(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GamePhase != CatanGamePhase.Main && game.GameSubPhase != CatanGameSubPhase.RollOrPlayDevelopmentCard
            && game.GameSubPhase != CatanGameSubPhase.Roll)
        {
            return Result.Failure<List<int>>(CatanErrors.InvalidGamePhase);
        }

        game.RollDiceAndDistributeResourcesToPlayers();

        var rollResult = game.LastRoll;

        return Result.Success(rollResult);
    }

    public Result EndTurn(string gameId)
    {
        var gameResult = GetGame(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GamePhase == CatanGamePhase.Main
        && game.GameSubPhase != CatanGameSubPhase.PlayTurn
        && game.GameSubPhase != CatanGameSubPhase.TradeOrBuild)
        {
            return Result.Failure(CatanErrors.InvalidGamePhase);
        }

        game.NextPlayer();

        return Result.Success();
    }

    public Result BuildRoad(string gameId, int firstX, int firstY, int secondX, int secondY)
    {
        var gameResult = GetGame(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GameSubPhase != CatanGameSubPhase.BuildRoad
        && game.GameSubPhase != CatanGameSubPhase.PlayTurn
        && game.GameSubPhase != CatanGameSubPhase.TradeOrBuild)
        {
            return Result.Failure(CatanErrors.InvalidGamePhase);
        }

        bool buildSuccess = false;

        if (game.GamePhase == CatanGamePhase.FirstRoundSetup
        || game.GamePhase == CatanGamePhase.SecondRoundSetup)
        {
            buildSuccess = game.BuildFreeRoad(new(firstX, firstY), new(secondX, secondY));
        }
        else if (game.GamePhase == CatanGamePhase.Main)
        {
            buildSuccess = game.BuildRoad(new(firstX, firstY), new(secondX, secondY));
        }

        if (!buildSuccess)
        {
            return Result.Failure(CatanErrors.InvalidBuildLocation);
        }

        return Result.Success();
    }

    public Result BuildSettlement(string gameId, int x, int y)
    {
        var gameResult = GetGame(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GameSubPhase != CatanGameSubPhase.BuildSettlement
        && game.GameSubPhase != CatanGameSubPhase.PlayTurn
        && game.GameSubPhase != CatanGameSubPhase.TradeOrBuild)
        {
            return Result.Failure(CatanErrors.InvalidGamePhase);
        }

        bool buildSuccess = false;

        if (game.GamePhase == CatanGamePhase.FirstRoundSetup
        || game.GamePhase == CatanGamePhase.SecondRoundSetup)
        {
            buildSuccess = game.BuildFreeSettlement(new(x, y));
        }
        else if (game.GamePhase == CatanGamePhase.Main)
        {
            buildSuccess = game.BuildSettlement(new(x, y));
        }

        if (!buildSuccess)
        {
            return Result.Failure(CatanErrors.InvalidBuildLocation);
        }

        return Result.Success();
    }

    public Result BuildCity(string gameId, int x, int y)
    {
        var gameResult = GetGame(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GameSubPhase != CatanGameSubPhase.PlayTurn
        && game.GameSubPhase != CatanGameSubPhase.TradeOrBuild)
        {
            return Result.Failure(CatanErrors.InvalidGamePhase);
        }

        var buildSuccess = game.BuildCity(new(x, y));

        if (!buildSuccess)
        {
            return Result.Failure(CatanErrors.InvalidBuildLocation);
        }

        return Result.Success();
    }

    public Result BuyDevelopmentCard(string gameId)
    {
        var gameResult = GetGame(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GameSubPhase != CatanGameSubPhase.PlayTurn
        && game.GameSubPhase != CatanGameSubPhase.TradeOrBuild)
        {
            return Result.Failure(CatanErrors.InvalidGamePhase);
        }

        var buySuccess = game.BuyDevelopmentCard();

        if (!buySuccess)
        {
            return Result.Failure(CatanErrors.CannotBuyDevelopmentCard);
        }

        return Result.Success();
    }

    public Result PlayKnightCard(string gameId, int x, int y, int playerColourToStealFrom)
    {
        var gameResult = GetGame(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (!IsValidPlayerColour(playerColourToStealFrom))
        {
            return Result.Failure<PlayerSpecificGameStatusResponse>(CatanErrors.InvalidPlayerColour);
        }

        if (game.GameSubPhase != CatanGameSubPhase.PlayTurn
        && game.GameSubPhase != CatanGameSubPhase.TradeOrBuild)
        {
            return Result.Failure(CatanErrors.InvalidGamePhase);
        }

        if (game.HasPlayedDevelopmentCardThisTurn)
        {
            return Result.Failure(CatanErrors.AlreadyPlayedDevelopmentCard);
        }

        var playSuccess = game.PlayKnightCard(new(x, y), (CatanPlayerColour)playerColourToStealFrom);

        if (!playSuccess)
        {
            return Result.Failure(CatanErrors.InvalidBuildLocation);
        }

        return Result.Success();
    }

    public Result PlayRoadBuildingCard(string gameId)
    {
        throw new NotImplementedException();
    }

    public Result PlayYearOfPlentyCard(string gameId, int resourceType1, int resourceType2)
    {
        throw new NotImplementedException();
    }

    public Result PlayMonopolyCard(string gameId, int resourceType)
    {
        throw new NotImplementedException();
    }

    public Result MoveRobber(string gameId, int x, int y)
    {
        throw new NotImplementedException();
    }

    public Result StealResource(string gameId, int victimColour, int resourceType)
    {
        throw new NotImplementedException();
    }

    public Result DiscardResources(string gameId, int playerColour, Dictionary<int, int> resources)
    {
        var gameResult = GetGame(gameId);

        if (gameResult.IsFailure)
        {
            return Result.Failure(gameResult.Error);
        }

        var game = gameResult.Value;

        if (game.GameSubPhase != CatanGameSubPhase.DiscardResources)
        {
            return Result.Failure(CatanErrors.InvalidGamePhase);
        }

        var catanResources = resources.ToDictionary(kvp => (CatanResourceType)kvp.Key, kvp => kvp.Value);

        var discardSuccess = game.DiscardResources((CatanPlayerColour)playerColour, catanResources);

        if (!discardSuccess)
        {
            return Result.Failure(CatanErrors.InvalidBuildLocation);
        }

        game.TryFinishDiscardingResources();

        return Result.Success();
    }

    public Result TradeWithBank(string gameId, int resourceToGive, int resourceToGet)
    {
        throw new NotImplementedException();
    }

    public Result EmbargoPlayer(string gameId, int playerColour, int playerColourToEmbargo)
    {
        throw new NotImplementedException();
    }

    private Result<CatanGame> GetGame(string gameId)
    {
        var game = currentGames.FirstOrDefault(g => g.Id == gameId);

        if (game is null)
        {
            return Result.Failure<CatanGame>(CatanErrors.GameNotFound);
        }

        return Result.Success(game);
    }

    private bool IsValidPlayerColour(int colour)
    {
        return colour >= 0 && colour <= 3;
    }
}
