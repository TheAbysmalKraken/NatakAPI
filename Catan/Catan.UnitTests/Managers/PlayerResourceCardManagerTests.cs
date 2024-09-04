using Catan.Domain.Enums;
using Catan.Domain.Managers;

namespace Catan.Domain.UnitTests.Managers;

public sealed class PlayerResourceCardManagerTests
{
    [Fact]
    public void RemoveRandom_Should_RemoveRandomCard_WhenCardsAvailable()
    {
        // Arrange
        var manager = new PlayerResourceCardManager();
        manager.Add(ResourceType.Brick, 3);
        manager.Add(ResourceType.Wood, 2);
        manager.Add(ResourceType.Sheep, 1);
        manager.Add(ResourceType.Wheat, 1);
        manager.Add(ResourceType.Ore, 1);

        // Act
        manager.RemoveRandom();

        // Assert
        Assert.True(manager.Cards.Values.Sum() == 7);
    }
}
