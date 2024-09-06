using Catan.Domain.Enums;
using Catan.Domain.Managers;

namespace Catan.Domain.UnitTests.Managers;

public sealed class BankTradeManagerTests
{
    [Fact]
    public void RemoveResourceCards_Should_RemoveResourceCards()
    {
        // Arrange
        var manager = new BankTradeManager();

        var existingCards = manager.ResourceCards.ToDictionary(x => x.Key, x => x.Value);
        var cardsToRemove = new Dictionary<ResourceType, int>()
        {
            { ResourceType.Wood, 1 },
            { ResourceType.Brick, 2 },
            { ResourceType.Sheep, 3 },
            { ResourceType.Wheat, 4 },
            { ResourceType.Ore, 5 }
        };

        // Act
        manager.RemoveResourceCards(cardsToRemove);

        // Assert
        foreach (var resourceType in existingCards.Keys)
        {
            var newCardCount = manager.ResourceCards[resourceType];

            Assert.Equal(existingCards[resourceType] - cardsToRemove[resourceType], newCardCount);
        }
    }

    [Fact]
    public void RemoveResourceCards_Should_ReturnRemovedCards()
    {
        // Arrange
        var manager = new BankTradeManager();

        var existingCards = manager.ResourceCards.ToDictionary(x => x.Key, x => x.Value);
        var cardsToRemove = new Dictionary<ResourceType, int>()
        {
            { ResourceType.Wood, 1 },
            { ResourceType.Brick, 2 },
            { ResourceType.Sheep, 3 },
            { ResourceType.Wheat, 4 },
            { ResourceType.Ore, 5 }
        };

        // Act
        var removedCards = manager.RemoveResourceCards(cardsToRemove);

        // Assert
        foreach (var resourceType in removedCards.Keys)
        {
            Assert.Equal(existingCards[resourceType] - manager.ResourceCards[resourceType], removedCards[resourceType]);
        }
    }

    [Fact]
    public void RemoveResourceCards_Should_OnlyRemoveUpToAsManyAsAreRemaining()
    {
        // Arrange
        var manager = new BankTradeManager();

        var existingCards = manager.ResourceCards.ToDictionary(x => x.Key, x => x.Value);
        var cardsToRemove = new Dictionary<ResourceType, int>()
        {
            { ResourceType.Wood, 20 },
            { ResourceType.Brick, 20 },
            { ResourceType.Sheep, 20 },
            { ResourceType.Wheat, 20 },
            { ResourceType.Ore, 20 }
        };

        // Act
        manager.RemoveResourceCards(cardsToRemove);

        // Assert
        foreach (var resourceType in existingCards.Keys)
        {
            Assert.Equal(0, manager.ResourceCards[resourceType]);
        }
    }

    [Fact]
    public void Trade_Should_ReturnFailure_WhenPlayerHasInsufficientResources()
    {
        // Arrange
        var manager = new BankTradeManager();
        var player = new Player(PlayerColour.Blue);

        // Act
        var tradeResult = manager.Trade(player, ResourceType.Wood, ResourceType.Brick);

        // Assert
        Assert.False(tradeResult.IsSuccess);
    }

    [Fact]
    public void Trade_Should_ReturnFailure_WhenBankHasInsufficientResources()
    {
        // Arrange
        var manager = new BankTradeManager();
        var player = new Player(PlayerColour.Blue);
        player.AddResourceCard(ResourceType.Wood, 4);

        manager.ResourceCards[ResourceType.Brick] = 0;

        // Act
        var tradeResult = manager.Trade(player, ResourceType.Wood, ResourceType.Brick);

        // Assert
        Assert.False(tradeResult.IsSuccess);
    }

    [Fact]
    public void Trade_Should_TradeFourToOne_WhenPlayerHasNoPorts()
    {
        // Arrange
        var manager = new BankTradeManager();
        var existingCards = manager.ResourceCards.ToDictionary(x => x.Key, x => x.Value);

        var player = new Player(PlayerColour.Blue);
        player.AddResourceCard(ResourceType.Wood, 4);

        // Act
        var tradeResult = manager.Trade(player, ResourceType.Wood, ResourceType.Brick);

        // Assert
        Assert.True(tradeResult.IsSuccess);
        Assert.Equal(0, player.CountResourceCard(ResourceType.Wood));
        Assert.Equal(1, player.CountResourceCard(ResourceType.Brick));
        Assert.Equal(existingCards[ResourceType.Wood] + 4, manager.ResourceCards[ResourceType.Wood]);
        Assert.Equal(existingCards[ResourceType.Brick] - 1, manager.ResourceCards[ResourceType.Brick]);
    }

    [Fact]
    public void Trade_Should_TradeThreeToOne_WhenPlayerHasThreeToOnePort()
    {
        // Arrange
        var manager = new BankTradeManager();
        var existingCards = manager.ResourceCards.ToDictionary(x => x.Key, x => x.Value);

        var player = new Player(PlayerColour.Blue);
        player.AddResourceCard(ResourceType.Wood, 3);

        player.Ports.Add(PortType.ThreeToOne);

        // Act
        var tradeResult = manager.Trade(player, ResourceType.Wood, ResourceType.Brick);

        // Assert
        Assert.True(tradeResult.IsSuccess);
        Assert.Equal(0, player.CountResourceCard(ResourceType.Wood));
        Assert.Equal(1, player.CountResourceCard(ResourceType.Brick));
        Assert.Equal(existingCards[ResourceType.Wood] + 3, manager.ResourceCards[ResourceType.Wood]);
        Assert.Equal(existingCards[ResourceType.Brick] - 1, manager.ResourceCards[ResourceType.Brick]);
    }

    [Fact]
    public void Trade_Should_TradeTwoToOne_WhenPlayerHasTwoToOnePort()
    {
        // Arrange
        var manager = new BankTradeManager();
        var existingCards = manager.ResourceCards.ToDictionary(x => x.Key, x => x.Value);

        var player = new Player(PlayerColour.Blue);
        player.AddResourceCard(ResourceType.Wood, 2);

        player.Ports.Add(PortType.Wood);

        // Act
        var tradeResult = manager.Trade(player, ResourceType.Wood, ResourceType.Brick);

        // Assert
        Assert.True(tradeResult.IsSuccess);
        Assert.Equal(0, player.CountResourceCard(ResourceType.Wood));
        Assert.Equal(1, player.CountResourceCard(ResourceType.Brick));
        Assert.Equal(existingCards[ResourceType.Wood] + 2, manager.ResourceCards[ResourceType.Wood]);
        Assert.Equal(existingCards[ResourceType.Brick] - 1, manager.ResourceCards[ResourceType.Brick]);
    }

    [Fact]
    public void Trade_Should_NotTradeTwoToOne_WhenPlayerHasWrongResourcePort()
    {
        // Arrange
        var manager = new BankTradeManager();
        var existingCards = manager.ResourceCards.ToDictionary(x => x.Key, x => x.Value);

        var player = new Player(PlayerColour.Blue);
        player.AddResourceCard(ResourceType.Wood, 2);

        player.Ports.Add(PortType.Brick);

        // Act
        var tradeResult = manager.Trade(player, ResourceType.Wood, ResourceType.Brick);

        // Assert
        Assert.False(tradeResult.IsSuccess);
        Assert.Equal(2, player.CountResourceCard(ResourceType.Wood));
        Assert.Equal(0, player.CountResourceCard(ResourceType.Brick));
        Assert.Equal(existingCards[ResourceType.Wood], manager.ResourceCards[ResourceType.Wood]);
        Assert.Equal(existingCards[ResourceType.Brick], manager.ResourceCards[ResourceType.Brick]);
    }
}
