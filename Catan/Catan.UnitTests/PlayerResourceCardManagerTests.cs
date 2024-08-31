using Catan.Domain.Enums;

namespace Catan.Domain.UnitTests;

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
        var result = manager.RemoveRandom();

        // Assert
        Assert.NotNull(result);
        Assert.True(manager.Cards.Values.Sum() == 7);
    }

    [Fact]
    public void RemoveRandom_Should_ReturnNull_WhenNoCardsAvailable()
    {
        // Arrange
        var manager = new PlayerResourceCardManager();

        // Act
        var result = manager.RemoveRandom();

        // Assert
        Assert.Null(result);
    }
}
