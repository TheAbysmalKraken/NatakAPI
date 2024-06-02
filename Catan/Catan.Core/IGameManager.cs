using Catan.Application.Models;

namespace Catan.Application;

public interface IGameManager
{
    Result TradeWithBank(string gameId, int resourceToGive, int resourceToGet);

    Result EmbargoPlayer(string gameId, int playerColour, int playerColourToEmbargo);
}
