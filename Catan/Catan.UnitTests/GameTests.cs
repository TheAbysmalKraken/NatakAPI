using Catan.Domain.Enums;

namespace Catan.Domain.UnitTests;

public class GameTests
{
    private readonly int defaultPlayerCount = 4;

    [Fact]
    public void CreateGame_CorrectNumberOfPlayers()
    {
        // Act
        var game = new Game(defaultPlayerCount);
        var players = game.GetPlayers();

        // Assert
        Assert.Equal(defaultPlayerCount, players.Count);
    }

    [Fact]
    public void CreateGame_DevelopmentCardsStartShuffled()
    {
        // Act
        var game1 = new Game(defaultPlayerCount);
        var game2 = new Game(defaultPlayerCount);

        var game1DevCards = game1.GetRemainingDevelopmentCards();
        var game2DevCards = game2.GetRemainingDevelopmentCards();

        // Assert
        Assert.NotEqual(game1DevCards, game2DevCards);
    }

    [Fact]
    public void NextPlayer_CurrentPlayerBecomesLastPlayerInList()
    {
        // Arrange
        var game = new Game(defaultPlayerCount);
        var currentPlayer = game.CurrentPlayer;

        game.BuildSettlement(new Point(2, 3), true);
        game.BuildRoad(new Point(2, 3), new Point(3, 3), true);

        // Act
        var result = game.NextPlayer();

        // Assert
        Assert.True(result.IsSuccess);
        var newCurrentPlayer = game.CurrentPlayer;
        Assert.NotEqual(currentPlayer, newCurrentPlayer);
    }

    [Fact]
    public void NextPlayer_CyclesThroughAllPlayers_ChangesToSecondSetupRound()
    {
        // Arrange
        var game = new Game(defaultPlayerCount);

        // Act
        for (int i = 0; i < defaultPlayerCount; i++)
        {
            game.BuildSettlement(new Point(2 + 2 * i, 3), true);
            game.BuildRoad(new Point(2 + 2 * i, 3), new Point(3 + 2 * i, 3), true);

            var result = game.NextPlayer();
            Assert.True(result.IsSuccess);
        }

        // Assert
        Assert.Equal(GameState.SecondSettlement, game.CurrentState);
        Assert.StrictEqual(game.GetPlayers().Last(), game.CurrentPlayer);
    }

    [Fact]
    public void NextPlayer_CyclesThroughAllPlayers_ChangesToMainGamePhase()
    {
        // Arrange
        var game = new Game(defaultPlayerCount);

        // Act
        for (int i = 0; i < defaultPlayerCount; i++)
        {
            game.BuildSettlement(new Point(2 + 2 * i, 3), true);
            game.BuildRoad(new Point(2 + 2 * i, 3), new Point(3 + 2 * i, 3), true);

            var result = game.NextPlayer();
            Assert.True(result.IsSuccess);
        }

        for (int i = 0; i < defaultPlayerCount; i++)
        {
            game.BuildSettlement(new Point(2 + 2 * i, 1), true);
            game.BuildRoad(new Point(2 + 2 * i, 1), new Point(3 + 2 * i, 1), true);

            var result = game.NextPlayer();
            Assert.True(result.IsSuccess);
        }

        // Assert
        Assert.Equal(GameState.BeforeRoll, game.CurrentState);
        Assert.StrictEqual(game.GetPlayers().First(), game.CurrentPlayer);
    }
}
