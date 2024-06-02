using Catan.Application.Models;

namespace Catan.Application;

public interface IGameManager
{
    Result EmbargoPlayer(string gameId, int playerColour, int playerColourToEmbargo);
}
