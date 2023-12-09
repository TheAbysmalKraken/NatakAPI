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
}
