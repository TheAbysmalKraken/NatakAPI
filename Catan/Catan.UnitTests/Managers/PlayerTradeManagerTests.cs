using Catan.Domain.Enums;
using Catan.Domain.Managers;

namespace Catan.Domain.UnitTests.Managers;

public sealed class PlayerTradeManagerTests
{
    [Fact]
    public void Inactive_Should_SetTradeOfferToInactive()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        SetupTrade(playerTradeManager);

        // Act
        playerTradeManager.Inactive();

        // Assert
        Assert.False(playerTradeManager.TradeOffer.IsActive);
    }

    [Fact]
    public void CreateOffer_Should_SetTradeOfferToActive()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        var player = new Player(PlayerColour.Red);
        player.AddResourceCards(
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 1 },
                { ResourceType.Wood, 1 },
                { ResourceType.Ore, 1 },
                { ResourceType.Wheat, 1 }
            }
        );

        // Act
        var result = playerTradeManager.CreateOffer(
            player,
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 1 },
                { ResourceType.Wood, 1 }
            },
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Ore, 1 },
                { ResourceType.Wheat, 1 }
            }
        );

        // Assert
        Assert.True(playerTradeManager.TradeOffer.IsActive);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void CreateOffer_Should_ReturnFailure_IfPlayerHasInsufficientResources()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        var player = new Player(PlayerColour.Red);
        player.AddResourceCards(
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 1 },
                { ResourceType.Ore, 1 },
                { ResourceType.Wheat, 1 }
            }
        );

        // Act
        var result = playerTradeManager.CreateOffer(
            player,
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 2 },
                { ResourceType.Wood, 1 }
            },
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Ore, 1 },
                { ResourceType.Wheat, 1 }
            }
        );

        // Assert
        Assert.False(playerTradeManager.TradeOffer.IsActive);
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CreateOffer_Should_AddEmbargoedPlayers_ToRejectedByList()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        var player = new Player(PlayerColour.Red);
        player.AddResourceCards(
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 1 },
                { ResourceType.Wood, 1 },
                { ResourceType.Ore, 1 },
                { ResourceType.Wheat, 1 }
            }
        );

        playerTradeManager.AddEmbargo(PlayerColour.Blue, PlayerColour.Red);
        playerTradeManager.AddEmbargo(PlayerColour.Red, PlayerColour.Green);

        // Act
        playerTradeManager.CreateOffer(
            player,
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 1 },
                { ResourceType.Wood, 1 }
            },
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Ore, 1 },
                { ResourceType.Wheat, 1 }
            }
        );

        // Assert
        Assert.Contains(PlayerColour.Blue, playerTradeManager.TradeOffer.RejectedBy);
        Assert.Contains(PlayerColour.Green, playerTradeManager.TradeOffer.RejectedBy);
    }

    [Fact]
    public void CancelOffer_Should_SetTradeOfferToInactive()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        SetupTrade(playerTradeManager);

        // Act
        playerTradeManager.CancelOffer();

        // Assert
        Assert.False(playerTradeManager.TradeOffer.IsActive);
    }

    [Fact]
    public void RejectOffer_Should_ReturnFailure_IfOfferingPlayerIsRejectingPlayer()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        SetupTrade(playerTradeManager);

        // Act
        var result = playerTradeManager.RejectOffer(PlayerColour.Red);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void RejectOffer_Should_ReturnFailure_IfTradeOfferIsNotActive()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        SetupTrade(playerTradeManager);

        playerTradeManager.CancelOffer();

        // Act
        var result = playerTradeManager.RejectOffer(PlayerColour.Blue);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void RejectOffer_Should_ReturnFailure_IfPlayerHasAlreadyRejectedTrade()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        SetupTrade(playerTradeManager);

        playerTradeManager.RejectOffer(PlayerColour.Blue);

        // Act
        var result = playerTradeManager.RejectOffer(PlayerColour.Blue);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void RejectOffer_Should_SetTradeOfferToInactive_IfAllPlayersHaveRejectedTrade()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        SetupTrade(playerTradeManager);

        playerTradeManager.RejectOffer(PlayerColour.Blue);
        playerTradeManager.RejectOffer(PlayerColour.Green);

        // Act
        var result = playerTradeManager.RejectOffer(PlayerColour.Yellow);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(playerTradeManager.TradeOffer.IsActive);
    }

    [Fact]
    public void RejectOffer_Should_AddPlayerToRejectedByList_IfPlayerHasNotAlreadyRejectedTrade()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        SetupTrade(playerTradeManager);

        // Act
        var result = playerTradeManager.RejectOffer(PlayerColour.Blue);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(PlayerColour.Blue, playerTradeManager.TradeOffer.RejectedBy);
    }

    [Fact]
    public void AcceptOffer_Should_ReturnFailure_WhenTradeOfferIsInactive()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        var player = new Player(PlayerColour.Red);
        var acceptingPlayer = new Player(PlayerColour.Blue);
        acceptingPlayer.AddResourceCards(
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 10 },
                { ResourceType.Wood, 10 },
                { ResourceType.Sheep, 10 },
                { ResourceType.Ore, 10 },
                { ResourceType.Wheat, 10 }
            }
        );

        // Act
        var result = playerTradeManager.AcceptOffer(player, acceptingPlayer);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void AcceptOffer_Should_ReturnFailure_WhenAcceptingPlayerIsOfferingPlayer()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        var player = new Player(PlayerColour.Red);
        SetupTrade(playerTradeManager);

        // Act
        var result = playerTradeManager.AcceptOffer(player, player);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void AcceptOffer_Should_ReturnFailure_WhenAcceptingPlayerHasInsufficientResources()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        var player = new Player(PlayerColour.Red);
        var acceptingPlayer = new Player(PlayerColour.Blue);
        SetupTrade(playerTradeManager);

        // Act
        var result = playerTradeManager.AcceptOffer(player, acceptingPlayer);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void AcceptOffer_Should_ReturnFailure_WhenAcceptingPlayerHasRejectedTrade()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        var player = new Player(PlayerColour.Red);
        var acceptingPlayer = new Player(PlayerColour.Blue);
        acceptingPlayer.AddResourceCards(
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 10 },
                { ResourceType.Wood, 10 },
                { ResourceType.Sheep, 10 },
                { ResourceType.Ore, 10 },
                { ResourceType.Wheat, 10 }
            }
        );
        SetupTrade(playerTradeManager);

        playerTradeManager.RejectOffer(acceptingPlayer.Colour);

        // Act
        var result = playerTradeManager.AcceptOffer(player, acceptingPlayer);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void AcceptOffer_Should_CompleteTrade()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        var player = new Player(PlayerColour.Red);
        var acceptingPlayer = new Player(PlayerColour.Blue);
        acceptingPlayer.AddResourceCards(
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 10 },
                { ResourceType.Wood, 10 },
                { ResourceType.Sheep, 10 },
                { ResourceType.Ore, 10 },
                { ResourceType.Wheat, 10 }
            }
        );

        SetupTrade(playerTradeManager);

        // Act
        var result = playerTradeManager.AcceptOffer(player, acceptingPlayer);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, player.CountResourceCard(ResourceType.Brick));
        Assert.Equal(0, player.CountResourceCard(ResourceType.Wood));
        Assert.Equal(9, acceptingPlayer.CountResourceCard(ResourceType.Ore));
        Assert.Equal(9, acceptingPlayer.CountResourceCard(ResourceType.Wheat));
    }

    [Fact]
    public void AcceptOffer_Should_SetTradeOfferToInactive()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        var player = new Player(PlayerColour.Red);
        var acceptingPlayer = new Player(PlayerColour.Blue);
        acceptingPlayer.AddResourceCards(
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 10 },
                { ResourceType.Wood, 10 },
                { ResourceType.Sheep, 10 },
                { ResourceType.Ore, 10 },
                { ResourceType.Wheat, 10 }
            }
        );

        SetupTrade(playerTradeManager);

        // Act
        var result = playerTradeManager.AcceptOffer(player, acceptingPlayer);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(playerTradeManager.TradeOffer.IsActive);
    }

    [Fact]
    public void AddEmbargo_Should_AddEmbargoedPlayerToEmbargoedPlayersList()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);

        // Act
        var result = playerTradeManager.AddEmbargo(PlayerColour.Blue, PlayerColour.Red);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(PlayerColour.Red, playerTradeManager.Embargoes[PlayerColour.Blue]);
    }

    [Fact]
    public void AddEmbargo_Should_ReturnFailure_IfEmbargoedPlayerIsEmbargoingPlayer()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);

        // Act
        var result = playerTradeManager.AddEmbargo(PlayerColour.Blue, PlayerColour.Blue);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void AddEmbargo_Should_ReturnFailure_IfEmbargoedPlayerIsAlreadyEmbargoed()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);

        playerTradeManager.AddEmbargo(PlayerColour.Blue, PlayerColour.Red);

        // Act
        var result = playerTradeManager.AddEmbargo(PlayerColour.Blue, PlayerColour.Red);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void RemoveEmbargo_Should_RemoveEmbargoedPlayerFromEmbargoedPlayersList()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);

        playerTradeManager.AddEmbargo(PlayerColour.Blue, PlayerColour.Red);

        // Act
        var result = playerTradeManager.RemoveEmbargo(PlayerColour.Blue, PlayerColour.Red);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(PlayerColour.Red, playerTradeManager.Embargoes[PlayerColour.Blue]);
    }

    [Fact]
    public void RemoveEmbargo_Should_ReturnFailure_IfEmbargoedPlayerIsNotEmbargoed()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);

        // Act
        var result = playerTradeManager.RemoveEmbargo(PlayerColour.Blue, PlayerColour.Red);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void UpdateTradeRejectionsFromEmbargoes_Should_AddEmbargoedPlayersToRejectedByList()
    {
        // Arrange
        var playerTradeManager = new PlayerTradeManager(4);
        var player = new Player(PlayerColour.Red);
        player.AddResourceCards(
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 1 },
                { ResourceType.Wood, 1 },
                { ResourceType.Ore, 1 },
                { ResourceType.Wheat, 1 }
            }
        );

        playerTradeManager.AddEmbargo(PlayerColour.Blue, PlayerColour.Red);
        playerTradeManager.AddEmbargo(PlayerColour.Red, PlayerColour.Green);

        // Act
        playerTradeManager.CreateOffer(
            player,
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 1 },
                { ResourceType.Wood, 1 }
            },
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Ore, 1 },
                { ResourceType.Wheat, 1 }
            }
        );

        // Assert
        Assert.Contains(PlayerColour.Blue, playerTradeManager.TradeOffer.RejectedBy);
        Assert.Contains(PlayerColour.Green, playerTradeManager.TradeOffer.RejectedBy);
    }

    private static void SetupTrade(PlayerTradeManager playerTradeManager)
    {
        var player = new Player(PlayerColour.Red);
        player.AddResourceCards(
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 1 },
                { ResourceType.Wood, 1 },
                { ResourceType.Ore, 1 },
                { ResourceType.Wheat, 1 }
            }
        );

        playerTradeManager.CreateOffer(
            player,
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Brick, 1 },
                { ResourceType.Wood, 1 }
            },
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Ore, 1 },
                { ResourceType.Wheat, 1 }
            }
        );
    }
}
