using Catan.Domain.Enums;

namespace Catan.Domain.UnitTests;

public class PlayerTests
{
    private readonly Player testPlayer;

    public PlayerTests()
    {
        testPlayer = new(PlayerColour.Red);
    }

    [Fact]
    public void BuyDevelopmentCard_VictoryPointCardAdded_VictoryPointAddedToCount()
    {
        // Arrange
        testPlayer.AddResourceCards(new Dictionary<ResourceType, int>
        {
            { ResourceType.Ore, 1 },
            { ResourceType.Sheep, 1 },
            { ResourceType.Wheat, 1 }
        });

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
        // Arrange
        testPlayer.AddResourceCards(new Dictionary<ResourceType, int>
        {
            { ResourceType.Ore, 1 },
            { ResourceType.Sheep, 1 },
            { ResourceType.Wheat, 1 }
        });

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

        testPlayer.AddResourceCards(new Dictionary<ResourceType, int>
        {
            { ResourceType.Ore, cardsToAdd },
            { ResourceType.Sheep, cardsToAdd },
            { ResourceType.Wheat, cardsToAdd }
        });

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

        testPlayer.AddResourceCards(new Dictionary<ResourceType, int>
        {
            { ResourceType.Ore, cardsToAdd },
            { ResourceType.Sheep, cardsToAdd },
            { ResourceType.Wheat, cardsToAdd }
        });

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
    public void CanPlayDevelopmentCardOfType_VictoryPointReturnsFailure()
    {
        // Arrange
        testPlayer.AddResourceCards(new Dictionary<ResourceType, int>
        {
            { ResourceType.Ore, 1 },
            { ResourceType.Sheep, 1 },
            { ResourceType.Wheat, 1 }
        });

        testPlayer.BuyDevelopmentCard(DevelopmentCardType.VictoryPoint);

        // Act
        var result = testPlayer.CanRemoveDevelopmentCard(DevelopmentCardType.VictoryPoint);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData(DevelopmentCardType.RoadBuilding)]
    [InlineData(DevelopmentCardType.YearOfPlenty)]
    [InlineData(DevelopmentCardType.Monopoly)]
    public void CanPlayDevelopmentCardOfType_HasNoneOfThatType_ReturnsFailure(
        DevelopmentCardType cardType)
    {
        // Arrange
        testPlayer.AddResourceCards(new Dictionary<ResourceType, int>
        {
            { ResourceType.Ore, 1 },
            { ResourceType.Sheep, 1 },
            { ResourceType.Wheat, 1 }
        });

        testPlayer.BuyDevelopmentCard(DevelopmentCardType.Knight);
        testPlayer.MoveOnHoldDevelopmentCardsToPlayable();
        testPlayer.BuyDevelopmentCard(cardType);

        // Act
        var result = testPlayer.CanRemoveDevelopmentCard(cardType);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData(DevelopmentCardType.Knight)]
    [InlineData(DevelopmentCardType.RoadBuilding)]
    [InlineData(DevelopmentCardType.YearOfPlenty)]
    [InlineData(DevelopmentCardType.Monopoly)]
    public void CanPlayDevelopmentCardOfType_HasSomeOfThatType_ReturnsSuccess(
        DevelopmentCardType cardType)
    {
        // Arrange
        testPlayer.AddResourceCards(new Dictionary<ResourceType, int>
        {
            { ResourceType.Ore, 1 },
            { ResourceType.Sheep, 1 },
            { ResourceType.Wheat, 1 }
        });

        testPlayer.BuyDevelopmentCard(cardType);
        testPlayer.MoveOnHoldDevelopmentCardsToPlayable();

        // Act
        var result = testPlayer.CanRemoveDevelopmentCard(cardType);

        // Assert
        Assert.True(result.IsSuccess);
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
        testPlayer.AddResourceCards(new Dictionary<ResourceType, int>
        {
            { ResourceType.Ore, 2 },
            { ResourceType.Sheep, 2 },
            { ResourceType.Wheat, 2 }
        });

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
