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
    public void AddDevelopmentCard_VictoryPointAddedToPlayable()
    {
        // Act
        testPlayer.AddDevelopmentCard(CatanDevelopmentCardType.VictoryPoint);

        // Assert
        Assert.Equal(0, testPlayer.GetDevelopmentCardsOnHold()[CatanDevelopmentCardType.VictoryPoint]);
        Assert.Equal(1, testPlayer.GetPlayableDevelopmentCards()[CatanDevelopmentCardType.VictoryPoint]);
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
}
