using Catan.Domain.Enums;
using Catan.Domain.Managers;

namespace Catan.Domain.UnitTests.Managers;

public sealed class PlayerDevelopmentCardManagerTests
{
    [Fact]
    public void HasOnHold_Should_ReturnTrue_WhenCardIsOnHold()
    {
        // Arrange
        var manager = new PlayerDevelopmentCardManager();
        manager.Add(DevelopmentCardType.Knight);

        // Act
        var result = manager.HasOnHold(DevelopmentCardType.Knight);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasOnHold_Should_ReturnFalse_WhenCardIsNotOnHold()
    {
        // Arrange
        var manager = new PlayerDevelopmentCardManager();
        manager.Add(DevelopmentCardType.Knight);
        manager.CycleOnHoldCards();

        // Act
        var result = manager.HasOnHold(DevelopmentCardType.Knight);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CountOnHold_Should_ReturnCountOfCardOnHold()
    {
        // Arrange
        var manager = new PlayerDevelopmentCardManager();
        manager.Add(DevelopmentCardType.Knight);
        manager.Add(DevelopmentCardType.Knight);

        // Act
        var result = manager.CountOnHold(DevelopmentCardType.Knight);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void CountAllOnHold_Should_ReturnCountOfAllCardsOnHold()
    {
        // Arrange
        var manager = new PlayerDevelopmentCardManager();
        manager.Add(DevelopmentCardType.Knight);
        manager.Add(DevelopmentCardType.Monopoly);
        manager.Add(DevelopmentCardType.RoadBuilding);
        manager.Add(DevelopmentCardType.YearOfPlenty);

        // Act
        var result = manager.CountAllOnHold();

        // Assert
        Assert.Equal(4, result);
    }

    [Theory]
    [InlineData(DevelopmentCardType.Knight)]
    [InlineData(DevelopmentCardType.Monopoly)]
    [InlineData(DevelopmentCardType.RoadBuilding)]
    [InlineData(DevelopmentCardType.YearOfPlenty)]
    public void Add_Should_AddCardToOnHold(DevelopmentCardType card)
    {
        // Arrange
        var manager = new PlayerDevelopmentCardManager();

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
        var manager = new PlayerDevelopmentCardManager();

        // Act
        manager.Add(DevelopmentCardType.VictoryPoint);

        // Assert
        Assert.True(manager.Cards[DevelopmentCardType.VictoryPoint] == 1);
        Assert.False(manager.OnHoldCards.ContainsKey(DevelopmentCardType.VictoryPoint));
    }

    [Fact]
    public void CycleOnHoldCards_Should_MoveAllOnHoldCardsToCards()
    {
        // Arrange
        var manager = new PlayerDevelopmentCardManager();
        manager.Add(DevelopmentCardType.Knight);
        manager.Add(DevelopmentCardType.Monopoly);
        manager.Add(DevelopmentCardType.RoadBuilding);
        manager.Add(DevelopmentCardType.YearOfPlenty);

        // Act
        manager.CycleOnHoldCards();

        // Assert
        Assert.True(manager.Cards[DevelopmentCardType.Knight] == 1);
        Assert.True(manager.Cards[DevelopmentCardType.Monopoly] == 1);
        Assert.True(manager.Cards[DevelopmentCardType.RoadBuilding] == 1);
        Assert.True(manager.Cards[DevelopmentCardType.YearOfPlenty] == 1);
        Assert.True(manager.OnHoldCards.Count == 0);
    }
}
