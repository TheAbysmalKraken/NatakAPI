using Catan.Domain;

namespace Catan.Core.UnitTests.GameActions.Shared;

public static class GameData
{
    public static Game Create(int numPlayers) => new(numPlayers);
}
