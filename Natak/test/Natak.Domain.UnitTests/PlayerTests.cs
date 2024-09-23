using Natak.Domain.Enums;

namespace Natak.Domain.UnitTests;

public class PlayerTests
{
    private readonly Player testPlayer;

    public PlayerTests()
    {
        testPlayer = new(PlayerColour.Red);
    }

    [Fact]
    public void AddDevelopmentCard_VictoryPointCardAdded_VictoryPointAddedToCount()
    {
        // Arrange
        testPlayer.AddResourceCards(new Dictionary<ResourceType, int>
        {
            { ResourceType.Ore, 1 },
            { ResourceType.Sheep, 1 },
            { ResourceType.Wheat, 1 }
        });

        // Act
        testPlayer.AddDevelopmentCard(DevelopmentCardType.VictoryPoint);

        // Assert
        Assert.Equal(1, testPlayer.ScoreManager.HiddenPoints);
    }
}
