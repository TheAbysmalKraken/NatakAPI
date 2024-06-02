using Catan.Application.Models;

namespace Catan.Application;

public interface IGameManager
{
    Result PlayMonopolyCard(string gameId, int resourceType);

    Result MoveRobber(string gameId, int x, int y);

    Result StealResource(string gameId, int victimColour);

    Result DiscardResources(string gameId, int playerColour, Dictionary<int, int> resources);

    Result TradeWithBank(string gameId, int resourceToGive, int resourceToGet);

    Result EmbargoPlayer(string gameId, int playerColour, int playerColourToEmbargo);
}
