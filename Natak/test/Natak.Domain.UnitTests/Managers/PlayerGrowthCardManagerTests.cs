using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Domain.UnitTests.Managers;

public sealed class PlayerGrowthCardManagerTests
{
    [Fact]
    public void HasOnHold_Should_ReturnTrue_WhenCardIsOnHold()
    {
        // Arrange
        var manager = new PlayerGrowthCardManager();
        manager.Add(GrowthCardType.Soldier);

        // Act
        var result = manager.HasOnHold(GrowthCardType.Soldier);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasOnHold_Should_ReturnFalse_WhenCardIsNotOnHold()
    {
        // Arrange
        var manager = new PlayerGrowthCardManager();
        manager.Add(GrowthCardType.Soldier);
        manager.CycleOnHoldCards();

        // Act
        var result = manager.HasOnHold(GrowthCardType.Soldier);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CountOnHold_Should_ReturnCountOfCardOnHold()
    {
        // Arrange
        var manager = new PlayerGrowthCardManager();
        manager.Add(GrowthCardType.Soldier);
        manager.Add(GrowthCardType.Soldier);

        // Act
        var result = manager.CountOnHold(GrowthCardType.Soldier);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void CountAllOnHold_Should_ReturnCountOfAllCardsOnHold()
    {
        // Arrange
        var manager = new PlayerGrowthCardManager();
        manager.Add(GrowthCardType.Soldier);
        manager.Add(GrowthCardType.Gatherer);
        manager.Add(GrowthCardType.Roaming);
        manager.Add(GrowthCardType.Wealth);

        // Act
        var result = manager.CountAllOnHold();

        // Assert
        Assert.Equal(4, result);
    }

    [Theory]
    [InlineData(GrowthCardType.Soldier)]
    [InlineData(GrowthCardType.Gatherer)]
    [InlineData(GrowthCardType.Roaming)]
    [InlineData(GrowthCardType.Wealth)]
    public void Add_Should_AddCardToOnHold(GrowthCardType card)
    {
        // Arrange
        var manager = new PlayerGrowthCardManager();

        // Act
        manager.Add(card);

        // Assert
        Assert.True(manager.OnHoldCards[card] == 1);
        Assert.False(manager.Cards.ContainsKey(card));
    }

    [Fact]
    public void Add_Should_AddVictoryPointToCards()
    {
        // Arrange
        var manager = new PlayerGrowthCardManager();

        // Act
        manager.Add(GrowthCardType.VictoryPoint);

        // Assert
        Assert.True(manager.Cards[GrowthCardType.VictoryPoint] == 1);
        Assert.False(manager.OnHoldCards.ContainsKey(GrowthCardType.VictoryPoint));
    }

    [Fact]
    public void CycleOnHoldCards_Should_MoveAllOnHoldCardsToCards()
    {
        // Arrange
        var manager = new PlayerGrowthCardManager();
        manager.Add(GrowthCardType.Soldier);
        manager.Add(GrowthCardType.Gatherer);
        manager.Add(GrowthCardType.Roaming);
        manager.Add(GrowthCardType.Wealth);

        // Act
        manager.CycleOnHoldCards();

        // Assert
        Assert.True(manager.Cards[GrowthCardType.Soldier] == 1);
        Assert.True(manager.Cards[GrowthCardType.Gatherer] == 1);
        Assert.True(manager.Cards[GrowthCardType.Roaming] == 1);
        Assert.True(manager.Cards[GrowthCardType.Wealth] == 1);
        Assert.True(manager.OnHoldCards.Count == 0);
    }
}
