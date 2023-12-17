using Catan.Domain;
using static Catan.Common.Enumerations;

namespace Catan.UnitTests.Domain;

public class CatanPlayerTests
{
    private readonly CatanPlayer testPlayer;

    public CatanPlayerTests()
    {
        testPlayer = new(CatanPlayerColour.Red);
    }

    [Fact]
    public void AddDevelopmentCard_VictoryPointAddedToCount()
    {
        // Act
        testPlayer.AddDevelopmentCard(CatanDevelopmentCardType.VictoryPoint);

        // Assert
        Assert.Equal(1, testPlayer.VictoryPoints);
    }

    [Theory]
    [InlineData(CatanDevelopmentCardType.Knight)]
    [InlineData(CatanDevelopmentCardType.RoadBuilding)]
    [InlineData(CatanDevelopmentCardType.YearOfPlenty)]
    [InlineData(CatanDevelopmentCardType.Monopoly)]
    public void AddDevelopmentCard_DevelopmentCardAddedToOnHold(CatanDevelopmentCardType cardType)
    {
        // Act
        testPlayer.AddDevelopmentCard(cardType);

        // Assert
        Assert.Equal(1, testPlayer.GetDevelopmentCardsOnHold()[cardType]);
        Assert.Equal(0, testPlayer.GetPlayableDevelopmentCards()[cardType]);
    }

    [Theory]
    [InlineData(CatanDevelopmentCardType.Knight)]
    [InlineData(CatanDevelopmentCardType.RoadBuilding)]
    [InlineData(CatanDevelopmentCardType.YearOfPlenty)]
    [InlineData(CatanDevelopmentCardType.Monopoly)]
    public void MoveOnHoldDevelopmentCardsToPlayable_CurrentPlayerDevelopmentCardsOnHoldSetToZero(
        CatanDevelopmentCardType cardType)
    {
        // Arrange
        var cardsToAdd = 3;

        for (var i = 0; i < cardsToAdd; i++)
        {
            testPlayer.AddDevelopmentCard(cardType);
        }

        // Act
        testPlayer.MoveOnHoldDevelopmentCardsToPlayable();

        // Assert
        Assert.Equal(0, testPlayer.GetDevelopmentCardsOnHold()[cardType]);
    }

    [Theory]
    [InlineData(CatanDevelopmentCardType.Knight)]
    [InlineData(CatanDevelopmentCardType.RoadBuilding)]
    [InlineData(CatanDevelopmentCardType.YearOfPlenty)]
    [InlineData(CatanDevelopmentCardType.Monopoly)]
    public void MoveOnHoldDevelopmentCardsToPlayable_CurrentPlayerDevelopmentCardsOnHoldBecomePlayable(
        CatanDevelopmentCardType cardType)
    {
        // Arrange
        var cardsToAdd = 3;

        for (var i = 0; i < cardsToAdd; i++)
        {
            testPlayer.AddDevelopmentCard(cardType);
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
        var canPlay = testPlayer.CanPlayDevelopmentCardOfType(CatanDevelopmentCardType.VictoryPoint);

        // Assert
        Assert.False(canPlay);
    }

    [Theory]
    [InlineData(CatanDevelopmentCardType.RoadBuilding)]
    [InlineData(CatanDevelopmentCardType.YearOfPlenty)]
    [InlineData(CatanDevelopmentCardType.Monopoly)]
    public void CanPlayDevelopmentCardOfType_HasNoneOfThatType_ReturnsFalse(
        CatanDevelopmentCardType cardType)
    {
        // Arrange
        testPlayer.AddDevelopmentCard(CatanDevelopmentCardType.Knight);
        testPlayer.MoveOnHoldDevelopmentCardsToPlayable();
        testPlayer.AddDevelopmentCard(cardType);

        // Act
        var canPlay = testPlayer.CanPlayDevelopmentCardOfType(cardType);

        // Assert
        Assert.False(canPlay);
    }

    [Theory]
    [InlineData(CatanDevelopmentCardType.Knight)]
    [InlineData(CatanDevelopmentCardType.RoadBuilding)]
    [InlineData(CatanDevelopmentCardType.YearOfPlenty)]
    [InlineData(CatanDevelopmentCardType.Monopoly)]
    public void CanPlayDevelopmentCardOfType_HasSomeOfThatType_ReturnsTrue(
        CatanDevelopmentCardType cardType)
    {
        // Arrange
        testPlayer.AddDevelopmentCard(cardType);
        testPlayer.MoveOnHoldDevelopmentCardsToPlayable();

        // Act
        var canPlay = testPlayer.CanPlayDevelopmentCardOfType(cardType);

        // Assert
        Assert.True(canPlay);
    }

    [Theory]
    [InlineData(CatanDevelopmentCardType.Knight)]
    [InlineData(CatanDevelopmentCardType.RoadBuilding)]
    [InlineData(CatanDevelopmentCardType.YearOfPlenty)]
    [InlineData(CatanDevelopmentCardType.Monopoly)]
    public void PlayDevelopmentCard_HasNoneOfThatType_CardCountIsZero(
        CatanDevelopmentCardType cardType)
    {
        // Act
        testPlayer.PlayDevelopmentCard(cardType);
        var cards = testPlayer.GetPlayableDevelopmentCards();

        // Assert
        Assert.Equal(0, cards[cardType]);
    }

    [Theory]
    [InlineData(CatanDevelopmentCardType.Knight)]
    [InlineData(CatanDevelopmentCardType.RoadBuilding)]
    [InlineData(CatanDevelopmentCardType.YearOfPlenty)]
    [InlineData(CatanDevelopmentCardType.Monopoly)]
    public void PlayDevelopmentCard_HasSomeOfThatType_CardCountOneLess(
        CatanDevelopmentCardType cardType)
    {
        // Arrange
        testPlayer.AddDevelopmentCard(cardType);
        testPlayer.AddDevelopmentCard(cardType);
        testPlayer.MoveOnHoldDevelopmentCardsToPlayable();

        var initialCardCount = testPlayer.GetPlayableDevelopmentCards()[cardType];

        // Act
        testPlayer.PlayDevelopmentCard(cardType);
        var cards = testPlayer.GetPlayableDevelopmentCards();

        // Assert
        Assert.Equal(initialCardCount - 1, cards[cardType]);
    }

    [Theory]
    [InlineData(2, 6)]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    [InlineData(9, 4)]
    public void SetVictoryPointsFromBuildings_CorrectPointsAdded(
        int settlementCount, int cityCount)
    {
        // Arrange
        var initialVictoryPoints = testPlayer.VictoryPoints;

        // Act
        testPlayer.SetVictoryPointsFromBuildings(settlementCount, cityCount);

        // Assert
        Assert.Equal(initialVictoryPoints + settlementCount + cityCount * 2, testPlayer.VictoryPoints);
    }
}
