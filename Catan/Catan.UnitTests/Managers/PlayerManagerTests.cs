using Catan.Domain.Enums;
using Catan.Domain.Managers;

namespace Catan.Domain.UnitTests.Managers;

public sealed class PlayerManagerTests
{
    [Fact]
    public void NextPlayer_Should_CycleToNextPlayer_WhenInSetup()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var currentPlayerColour = playerManager.CurrentPlayerColour;

        // Act
        playerManager.NextPlayer();

        // Assert
        Assert.NotEqual(currentPlayerColour, playerManager.CurrentPlayerColour);
    }

    [Fact]
    public void NextPlayer_Should_GiveLastPlayerTwoConsecutiveTurns_WhenInSetup()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        playerManager.NextPlayer();
        playerManager.NextPlayer();
        playerManager.NextPlayer();
        var currentPlayerColour = playerManager.CurrentPlayerColour;

        // Act
        playerManager.NextPlayer();

        // Assert
        Assert.Equal(currentPlayerColour, playerManager.CurrentPlayerColour);
    }

    [Fact]
    public void NextPlayer_Should_MoveInReverseOrderAfterLastPlayer_WhenInSetup()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        playerManager.NextPlayer();
        playerManager.NextPlayer();
        var thirdPlayerColour = playerManager.CurrentPlayerColour;
        playerManager.NextPlayer();
        playerManager.NextPlayer();

        // Act
        playerManager.NextPlayer();

        // Assert
        Assert.Equal(thirdPlayerColour, playerManager.CurrentPlayerColour);
    }

    [Fact]
    public void NextPlayer_Should_MoveOutOfSetup_WhenAtEndOfSetup()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        for (var i = 0; i < 7; i++)
        {
            playerManager.NextPlayer();
        }
        var isSetupBefore = playerManager.IsSetup;

        // Act
        playerManager.NextPlayer();

        // Assert
        var isSetupAfter = playerManager.IsSetup;
        Assert.True(isSetupBefore);
        Assert.False(isSetupAfter);
    }

    [Fact]
    public void NextPlayer_Should_StartWithFirstPlayer_WhenOutOfSetup()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var firstPlayerColour = playerManager.CurrentPlayerColour;

        // Act
        playerManager.IsSetup = false;

        // Assert
        Assert.Equal(firstPlayerColour, playerManager.CurrentPlayerColour);
    }

    [Fact]
    public void NextPlayer_Should_CycleToFirstPlayerAfterLastPlayer_WhenOutOfSetup()
    {
        // Arrange
        var playerManager = new PlayerManager(4)
        {
            IsSetup = false
        };
        var firstPlayerColour = playerManager.CurrentPlayerColour;

        // Act
        for (var i = 0; i < 4; i++)
        {
            playerManager.NextPlayer();
        }

        // Assert
        Assert.Equal(firstPlayerColour, playerManager.CurrentPlayerColour);
    }

    [Fact]
    public void NextPlayer_Should_CyclePlayerDevelopmentCards()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;
        player.AddDevelopmentCard(DevelopmentCardType.Knight);

        var hasKnightBefore = player.DevelopmentCardManager.Has(DevelopmentCardType.Knight);

        // Act
        playerManager.NextPlayer();

        // Assert
        var hasKnightAfter = player.DevelopmentCardManager.Has(DevelopmentCardType.Knight);
        Assert.False(hasKnightBefore);
        Assert.True(hasKnightAfter);
    }

    [Fact]
    public void GivePlayerMonopolyResource_Should_GiveResourceToPlayer_WhenOtherPlayersHaveResource()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;
        var otherPlayer = playerManager.Players.First(p => p.Colour != player.Colour);
        otherPlayer.AddResourceCard(ResourceType.Wood, 2);

        var playerWoodBefore = player.CountResourceCard(ResourceType.Wood);
        var otherPlayerWoodBefore = otherPlayer.CountResourceCard(ResourceType.Wood);

        // Act
        playerManager.GivePlayerMonopolyResource(player, ResourceType.Wood);

        // Assert
        var playerWoodAfter = player.CountResourceCard(ResourceType.Wood);
        var otherPlayerWoodAfter = otherPlayer.CountResourceCard(ResourceType.Wood);
        Assert.Equal(2, playerWoodAfter);
        Assert.Equal(0, otherPlayerWoodAfter);
        Assert.Equal(0, playerWoodBefore);
        Assert.Equal(2, otherPlayerWoodBefore);
    }

    [Fact]
    public void GivePlayerMonopolyResource_Should_NotGiveResourceToPlayer_WhenOtherPlayersDoNotHaveResource()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;
        var otherPlayer = playerManager.Players.First(p => p.Colour != player.Colour);

        var playerWoodBefore = player.CountResourceCard(ResourceType.Wood);
        var otherPlayerWoodBefore = otherPlayer.CountResourceCard(ResourceType.Wood);

        // Act
        playerManager.GivePlayerMonopolyResource(player, ResourceType.Wood);

        // Assert
        var playerWoodAfter = player.CountResourceCard(ResourceType.Wood);
        var otherPlayerWoodAfter = otherPlayer.CountResourceCard(ResourceType.Wood);
        Assert.Equal(0, playerWoodAfter);
        Assert.Equal(0, otherPlayerWoodAfter);
        Assert.Equal(0, playerWoodBefore);
        Assert.Equal(0, otherPlayerWoodBefore);
    }

    [Fact]
    public void StealFromPlayer_Should_StealResourceFromPlayer_WhenOtherPlayerHasResource()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;
        var otherPlayer = playerManager.Players.First(p => p.Colour != player.Colour);
        otherPlayer.AddResourceCard(ResourceType.Wood, 2);

        var playerWoodBefore = player.CountResourceCard(ResourceType.Wood);
        var otherPlayerWoodBefore = otherPlayer.CountResourceCard(ResourceType.Wood);

        // Act
        var result = playerManager.StealFromPlayer(player.Colour, otherPlayer.Colour);

        // Assert
        Assert.True(result.IsSuccess);
        var playerWoodAfter = player.CountResourceCard(ResourceType.Wood);
        var otherPlayerWoodAfter = otherPlayer.CountResourceCard(ResourceType.Wood);
        Assert.Equal(1, playerWoodAfter);
        Assert.Equal(1, otherPlayerWoodAfter);
        Assert.Equal(0, playerWoodBefore);
        Assert.Equal(2, otherPlayerWoodBefore);
    }

    [Fact]
    public void StealFromPlayer_Should_NotStealResourceFromPlayer_WhenOtherPlayerDoesNotHaveResource()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;
        var otherPlayer = playerManager.Players.First(p => p.Colour != player.Colour);

        var playerWoodBefore = player.CountResourceCard(ResourceType.Wood);
        var otherPlayerWoodBefore = otherPlayer.CountResourceCard(ResourceType.Wood);

        // Act
        var result = playerManager.StealFromPlayer(player.Colour, otherPlayer.Colour);

        // Assert
        Assert.True(result.IsSuccess);
        var playerWoodAfter = player.CountResourceCard(ResourceType.Wood);
        var otherPlayerWoodAfter = otherPlayer.CountResourceCard(ResourceType.Wood);
        Assert.Equal(0, playerWoodAfter);
        Assert.Equal(0, otherPlayerWoodAfter);
        Assert.Equal(0, playerWoodBefore);
        Assert.Equal(0, otherPlayerWoodBefore);
    }
}
