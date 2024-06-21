using Catan.Domain.Enums;

namespace Catan.Domain.UnitTests;

public class CatanGameTests
{
    private readonly int defaultPlayerCount = 4;

    [Fact]
    public void CreateCatanGame_CorrectNumberOfPlayers()
    {
        // Act
        var game = new Game(defaultPlayerCount);
        var players = game.GetPlayers();

        // Assert
        Assert.Equal(defaultPlayerCount, players.Count);
    }

    [Fact]
    public void CreateCatanGame_DevelopmentCardsStartShuffled()
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
    public void RollDice_IndividualDiceValuesWithinRange()
    {
        // Arrange
        var game = new Game(defaultPlayerCount);

        // Act
        game.RollDice();
        var rolledDice = game.GetRolledDice();

        // Assert
        foreach (var diceTotal in rolledDice)
        {
            Assert.InRange(diceTotal, 1, 6);
        }
    }

    [Fact]
    public void RollDice_DiceTotalCorrectlyCalculated()
    {
        // Arrange
        var game = new Game(defaultPlayerCount);

        // Act
        game.RollDice();
        var rolledDice = game.GetRolledDice();
        var expectedTotal = rolledDice.Sum();
        var actualTotal = game.DiceTotal;

        // Assert
        Assert.Equal(expectedTotal, actualTotal);
    }

    [Fact]
    public void NextPlayer_CurrentPlayerBecomesLastPlayerInList()
    {
        // Arrange
        var game = new Game(defaultPlayerCount);
        var currentPlayer = game.CurrentPlayer;

        // Act
        game.NextPlayer();

        // Assert
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
            game.NextPlayer();
        }

        // Assert
        Assert.Equal(GamePhase.SecondRoundSetup, game.GamePhase);
        Assert.StrictEqual(game.GetPlayers().Last(), game.CurrentPlayer);
    }

    [Fact]
    public void NextPlayer_CyclesThroughAllPlayers_ChangesToMainGamePhase()
    {
        // Arrange
        var game = new Game(defaultPlayerCount);

        // Act
        for (int i = 0; i < defaultPlayerCount * 2; i++)
        {
            game.NextPlayer();
        }

        // Assert
        Assert.Equal(GamePhase.Main, game.GamePhase);
        Assert.StrictEqual(game.GetPlayers().First(), game.CurrentPlayer);
    }
}