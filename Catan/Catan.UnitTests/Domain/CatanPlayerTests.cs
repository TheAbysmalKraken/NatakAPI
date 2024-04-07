using Catan.Domain;
using static Catan.Common.Enumerations;

namespace Catan.UnitTests.Domain;

public class CatanPlayerTests
{
    private readonly Player testPlayer;

    public CatanPlayerTests()
    {
        testPlayer = new(PlayerColour.Red);
    }

    [Fact]
    public void BuyDevelopmentCard_VictoryPointCardAdded_VictoryPointAddedToCount()
    {
        // Act
        testPlayer.BuyDevelopmentCard(DevelopmentCardType.VictoryPoint);

        // Assert
        Assert.Equal(1, testPlayer.VictoryPoints);
    }

    [Theory]
    [InlineData(DevelopmentCardType.Knight)]
    [InlineData(DevelopmentCardType.RoadBuilding)]
    [InlineData(DevelopmentCardType.YearOfPlenty)]
    [InlineData(DevelopmentCardType.Monopoly)]
    public void BuyDevelopmentCard_DevelopmentCardAddedToOnHold(DevelopmentCardType cardType)
    {
        // Act
        testPlayer.BuyDevelopmentCard(cardType);

        // Assert
        Assert.Equal(1, testPlayer.GetDevelopmentCardsOnHold()[cardType]);
        Assert.Equal(0, testPlayer.GetPlayableDevelopmentCards()[cardType]);
    }

    [Theory]
    [InlineData(DevelopmentCardType.Knight)]
    [InlineData(DevelopmentCardType.RoadBuilding)]
    [InlineData(DevelopmentCardType.YearOfPlenty)]
    [InlineData(DevelopmentCardType.Monopoly)]
    public void MoveOnHoldDevelopmentCardsToPlayable_CurrentPlayerDevelopmentCardsOnHoldSetToZero(
        DevelopmentCardType cardType)
    {
        // Arrange
        var cardsToAdd = 3;

        for (var i = 0; i < cardsToAdd; i++)
        {
            testPlayer.BuyDevelopmentCard(cardType);
        }

        // Act
        testPlayer.MoveOnHoldDevelopmentCardsToPlayable();

        // Assert
        Assert.Equal(0, testPlayer.GetDevelopmentCardsOnHold()[cardType]);
    }

    [Theory]
    [InlineData(DevelopmentCardType.Knight)]
    [InlineData(DevelopmentCardType.RoadBuilding)]
    [InlineData(DevelopmentCardType.YearOfPlenty)]
    [InlineData(DevelopmentCardType.Monopoly)]
    public void MoveOnHoldDevelopmentCardsToPlayable_CurrentPlayerDevelopmentCardsOnHoldBecomePlayable(
        DevelopmentCardType cardType)
    {
        // Arrange
        var cardsToAdd = 3;

        for (var i = 0; i < cardsToAdd; i++)
        {
            testPlayer.BuyDevelopmentCard(cardType);
        }

        // Act
        testPlayer.MoveOnHoldDevelopmentCardsToPlayable();

        // Assert
        Assert.Equal(0, testPlayer.GetDevelopmentCardsOnHold()[cardType]);
        Assert.Equal(cardsToAdd, testPlayer.GetPlayableDevelopmentCards()[cardType]);
    }

    [Fact]
    public void CanPlayDevelopmentCardOfType_VictoryPointReturnsFalse()
    {
        // Act
        var canPlay = testPlayer.CanRemoveDevelopmentCard(DevelopmentCardType.VictoryPoint);

        // Assert
        Assert.False(canPlay);
    }

    [Theory]
    [InlineData(DevelopmentCardType.RoadBuilding)]
    [InlineData(DevelopmentCardType.YearOfPlenty)]
    [InlineData(DevelopmentCardType.Monopoly)]
    public void CanPlayDevelopmentCardOfType_HasNoneOfThatType_ReturnsFalse(
        DevelopmentCardType cardType)
    {
        // Arrange
        testPlayer.BuyDevelopmentCard(DevelopmentCardType.Knight);
        testPlayer.MoveOnHoldDevelopmentCardsToPlayable();
        testPlayer.BuyDevelopmentCard(cardType);

        // Act
        var canPlay = testPlayer.CanRemoveDevelopmentCard(cardType);

        // Assert
        Assert.False(canPlay);
    }

    [Theory]
    [InlineData(DevelopmentCardType.Knight)]
    [InlineData(DevelopmentCardType.RoadBuilding)]
    [InlineData(DevelopmentCardType.YearOfPlenty)]
    [InlineData(DevelopmentCardType.Monopoly)]
    public void CanPlayDevelopmentCardOfType_HasSomeOfThatType_ReturnsTrue(
        DevelopmentCardType cardType)
    {
        // Arrange
        testPlayer.BuyDevelopmentCard(cardType);
        testPlayer.MoveOnHoldDevelopmentCardsToPlayable();

        // Act
        var canPlay = testPlayer.CanRemoveDevelopmentCard(cardType);

        // Assert
        Assert.True(canPlay);
    }

    [Theory]
    [InlineData(DevelopmentCardType.Knight)]
    [InlineData(DevelopmentCardType.RoadBuilding)]
    [InlineData(DevelopmentCardType.YearOfPlenty)]
    [InlineData(DevelopmentCardType.Monopoly)]
    public void PlayDevelopmentCard_HasNoneOfThatType_CardCountIsZero(
        DevelopmentCardType cardType)
    {
        // Act
        testPlayer.RemoveDevelopmentCard(cardType);
        var cards = testPlayer.GetPlayableDevelopmentCards();

        // Assert
        Assert.Equal(0, cards[cardType]);
    }

    [Theory]
    [InlineData(DevelopmentCardType.Knight)]
    [InlineData(DevelopmentCardType.RoadBuilding)]
    [InlineData(DevelopmentCardType.YearOfPlenty)]
    [InlineData(DevelopmentCardType.Monopoly)]
    public void PlayDevelopmentCard_HasSomeOfThatType_CardCountOneLess(
        DevelopmentCardType cardType)
    {
        // Arrange
        testPlayer.BuyDevelopmentCard(cardType);
        testPlayer.BuyDevelopmentCard(cardType);
        testPlayer.MoveOnHoldDevelopmentCardsToPlayable();

        var initialCardCount = testPlayer.GetPlayableDevelopmentCards()[cardType];

        // Act
        testPlayer.RemoveDevelopmentCard(cardType);
        var cards = testPlayer.GetPlayableDevelopmentCards();

        // Assert
        Assert.Equal(initialCardCount - 1, cards[cardType]);
    }
}
