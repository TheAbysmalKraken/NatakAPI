using Catan.Application.Models;

namespace Catan.Application;

public interface ICatanGameManager
{
    Result<PlayerSpecificGameStatusResponse> GetGameStatus(string gameId, int playerColour);

    Result<List<CoordinatesResponse>> GetAvailableSettlementLocations(string gameId, int playerColour);

    Result<List<CoordinatesResponse>> GetAvailableCityLocations(string gameId, int playerColour);

    Result<List<RoadCoordinatesResponse>> GetAvailableRoadLocations(string gameId, int playerColour);

    Result<string> CreateNewGame(int playerCount);

    Result<List<int>> RollDice(string gameId);

    Result EndTurn(string gameId);

    Result BuildRoad(string gameId, int firstX, int firstY, int secondX, int secondY);

    Result BuildSettlement(string gameId, int x, int y);

    Result BuildCity(string gameId, int x, int y);

    Result BuyDevelopmentCard(string gameId);

    Result PlayKnightCard(string gameId, int x, int y, int playerColourToStealFrom);

    Result PlayRoadBuildingCard(string gameId);

    Result PlayYearOfPlentyCard(string gameId, int resourceType1, int resourceType2);

    Result PlayMonopolyCard(string gameId, int resourceType);

    Result MoveRobber(string gameId, int x, int y);

    Result StealResource(string gameId, int victimColour, int resourceType);

    Result DiscardResources(string gameId, Dictionary<int, int> resources);

    Result TradeWithBank(string gameId, int resourceToGive, int resourceToGet);

    Result EmbargoPlayer(string gameId, int playerColour, int playerColourToEmbargo);
}
