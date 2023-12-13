using Catan.Domain;

namespace Catan.UnitTests.Domain;

public class CatanGameTests
{
    [Fact]
    public void CreateCatanGame_CorrectNumberOfPlayers()
    {
        // Arrange
        var numberOfPlayers = 4;

        // Act
        var game = new CatanGame(numberOfPlayers);
        var players = game.GetPlayers();

        // Assert
        Assert.Equal(numberOfPlayers, players.Count);
    }

    [Fact]
    public void CreateCatanGame_DevelopmentCardsStartShuffled()
    {
        // Arrange
        var numberOfPlayers = 4;

        // Act
        var game1 = new CatanGame(numberOfPlayers);
        var game2 = new CatanGame(numberOfPlayers);

        var game1DevCards = game1.GetRemainingDevelopmentCards();
        var game2DevCards = game2.GetRemainingDevelopmentCards();

        // Assert
        Assert.NotEqual(game1DevCards, game2DevCards);
    }
}
