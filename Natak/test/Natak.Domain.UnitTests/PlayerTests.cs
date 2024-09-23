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
    public void AddGrowthCard_VictoryPointCardAdded_VictoryPointAddedToCount()
    {
        // Arrange
        testPlayer.AddResourceCards(new Dictionary<ResourceType, int>
        {
            { ResourceType.Metal, 1 },
            { ResourceType.Animal, 1 },
            { ResourceType.Food, 1 }
        });

        // Act
        testPlayer.AddGrowthCard(GrowthCardType.VictoryPoint);

        // Assert
        Assert.Equal(1, testPlayer.ScoreManager.HiddenPoints);
    }
}
