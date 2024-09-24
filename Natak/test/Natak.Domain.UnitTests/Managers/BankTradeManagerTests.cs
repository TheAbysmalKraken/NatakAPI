using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Domain.UnitTests.Managers;

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
            { ResourceType.Clay, 2 },
            { ResourceType.Animal, 3 },
            { ResourceType.Food, 4 },
            { ResourceType.Metal, 5 }
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
            { ResourceType.Clay, 2 },
            { ResourceType.Animal, 3 },
            { ResourceType.Food, 4 },
            { ResourceType.Metal, 5 }
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
            { ResourceType.Clay, 20 },
            { ResourceType.Animal, 20 },
            { ResourceType.Food, 20 },
            { ResourceType.Metal, 20 }
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
        var tradeResult = manager.Trade(player, ResourceType.Wood, ResourceType.Clay);

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

        manager.ResourceCards[ResourceType.Clay] = 0;

        // Act
        var tradeResult = manager.Trade(player, ResourceType.Wood, ResourceType.Clay);

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
        var tradeResult = manager.Trade(player, ResourceType.Wood, ResourceType.Clay);

        // Assert
        Assert.True(tradeResult.IsSuccess);
        Assert.Equal(0, player.CountResourceCard(ResourceType.Wood));
        Assert.Equal(1, player.CountResourceCard(ResourceType.Clay));
        Assert.Equal(existingCards[ResourceType.Wood] + 4, manager.ResourceCards[ResourceType.Wood]);
        Assert.Equal(existingCards[ResourceType.Clay] - 1, manager.ResourceCards[ResourceType.Clay]);
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
        var tradeResult = manager.Trade(player, ResourceType.Wood, ResourceType.Clay);

        // Assert
        Assert.True(tradeResult.IsSuccess);
        Assert.Equal(0, player.CountResourceCard(ResourceType.Wood));
        Assert.Equal(1, player.CountResourceCard(ResourceType.Clay));
        Assert.Equal(existingCards[ResourceType.Wood] + 3, manager.ResourceCards[ResourceType.Wood]);
        Assert.Equal(existingCards[ResourceType.Clay] - 1, manager.ResourceCards[ResourceType.Clay]);
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
        var tradeResult = manager.Trade(player, ResourceType.Wood, ResourceType.Clay);

        // Assert
        Assert.True(tradeResult.IsSuccess);
        Assert.Equal(0, player.CountResourceCard(ResourceType.Wood));
        Assert.Equal(1, player.CountResourceCard(ResourceType.Clay));
        Assert.Equal(existingCards[ResourceType.Wood] + 2, manager.ResourceCards[ResourceType.Wood]);
        Assert.Equal(existingCards[ResourceType.Clay] - 1, manager.ResourceCards[ResourceType.Clay]);
    }

    [Fact]
    public void Trade_Should_NotTradeTwoToOne_WhenPlayerHasWrongResourcePort()
    {
        // Arrange
        var manager = new BankTradeManager();
        var existingCards = manager.ResourceCards.ToDictionary(x => x.Key, x => x.Value);

        var player = new Player(PlayerColour.Blue);
        player.AddResourceCard(ResourceType.Wood, 2);

        player.Ports.Add(PortType.Clay);

        // Act
        var tradeResult = manager.Trade(player, ResourceType.Wood, ResourceType.Clay);

        // Assert
        Assert.False(tradeResult.IsSuccess);
        Assert.Equal(2, player.CountResourceCard(ResourceType.Wood));
        Assert.Equal(0, player.CountResourceCard(ResourceType.Clay));
        Assert.Equal(existingCards[ResourceType.Wood], manager.ResourceCards[ResourceType.Wood]);
        Assert.Equal(existingCards[ResourceType.Clay], manager.ResourceCards[ResourceType.Clay]);
    }
}
