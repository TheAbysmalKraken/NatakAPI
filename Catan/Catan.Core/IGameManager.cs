using Catan.Application.Models;

namespace Catan.Application;

public interface IGameManager
{
    Result BuyDevelopmentCard(string gameId);

    Result PlayKnightCard(string gameId, int x, int y, int playerColourToStealFrom);

    Result PlayRoadBuildingCard(
        string gameId, int firstX, int firstY, int secondX, int secondY,
        int thirdX, int thirdY, int fourthX, int fourthY);

    Result PlayYearOfPlentyCard(string gameId, int resourceType1, int resourceType2);

    Result PlayMonopolyCard(string gameId, int resourceType);

    Result MoveRobber(string gameId, int x, int y);

    Result StealResource(string gameId, int victimColour);

    Result DiscardResources(string gameId, int playerColour, Dictionary<int, int> resources);

    Result TradeWithBank(string gameId, int resourceToGive, int resourceToGet);

    Result EmbargoPlayer(string gameId, int playerColour, int playerColourToEmbargo);
}
