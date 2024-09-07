using Catan.Domain;

namespace Catan.Core.UnitTests.GameActions.Shared;

internal static class GameFactory
{
    public static Game Create(GameFactoryOptions? options = null)
    {
        options ??= new GameFactoryOptions();

        var game = new Game(options.PlayerCount);

        if (!options.IsSetup)
        {
            for (var i = 0; i < options.PlayerCount; i++)
            {
                var x = 2 + 2 * i;
                game.PlaceSettlement(new(x, 3));
                game.PlaceRoad(new(x, 3), new(x, 2));
                game.EndTurn();
            }

            for (var i = 0; i < options.PlayerCount; i++)
            {
                var x = 3 + 2 * i;
                game.PlaceSettlement(new(x, 1));
                game.PlaceRoad(new(x, 1), new(x, 2));
                game.EndTurn();
            }
        }

        return game;
    }
}
