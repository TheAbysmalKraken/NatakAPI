using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Domain.UnitTests.Managers;

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
    public void NextPlayer_Should_CyclePlayerGrowthCards()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;
        player.AddGrowthCard(GrowthCardType.Soldier);

        var hasSoldierBefore = player.GrowthCardManager.Has(GrowthCardType.Soldier);

        // Act
        playerManager.NextPlayer();

        // Assert
        var hasSoldierAfter = player.GrowthCardManager.Has(GrowthCardType.Soldier);
        Assert.False(hasSoldierBefore);
        Assert.True(hasSoldierAfter);
    }

    [Fact]
    public void GivePlayerGathererResource_Should_GiveResourceToPlayer_WhenOtherPlayersHaveResource()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;
        var otherPlayer = playerManager.Players.First(p => p.Colour != player.Colour);
        otherPlayer.AddResourceCard(ResourceType.Wood, 2);

        var playerWoodBefore = player.CountResourceCard(ResourceType.Wood);
        var otherPlayerWoodBefore = otherPlayer.CountResourceCard(ResourceType.Wood);

        // Act
        playerManager.GivePlayerGathererResource(player, ResourceType.Wood);

        // Assert
        var playerWoodAfter = player.CountResourceCard(ResourceType.Wood);
        var otherPlayerWoodAfter = otherPlayer.CountResourceCard(ResourceType.Wood);
        Assert.Equal(2, playerWoodAfter);
        Assert.Equal(0, otherPlayerWoodAfter);
        Assert.Equal(0, playerWoodBefore);
        Assert.Equal(2, otherPlayerWoodBefore);
    }

    [Fact]
    public void GivePlayerGathererResource_Should_NotGiveResourceToPlayer_WhenOtherPlayersDoNotHaveResource()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;
        var otherPlayer = playerManager.Players.First(p => p.Colour != player.Colour);

        var playerWoodBefore = player.CountResourceCard(ResourceType.Wood);
        var otherPlayerWoodBefore = otherPlayer.CountResourceCard(ResourceType.Wood);

        // Act
        playerManager.GivePlayerGathererResource(player, ResourceType.Wood);

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

    [Fact]
    public void GivePort_Should_GivePlayerPort()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;

        // Act
        playerManager.GivePort(player.Colour, PortType.ThreeToOne);

        // Assert
        Assert.Contains(PortType.ThreeToOne, player.Ports);
    }

    [Fact]
    public void UpdateLongestRoadPlayer_Should_UpdatePlayerLongestRoad()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;

        // Act
        playerManager.UpdateLongestRoadPlayer(player.Colour);

        // Assert
        Assert.True(player.ScoreManager.HasLongestRoad);
    }

    [Fact]
    public void UpdateLargestArmyPlayer_Should_GiveLargestArmyCard_ToPlayerWithLargestArmy()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;
        player.AddGrowthCard(GrowthCardType.Soldier);
        player.AddGrowthCard(GrowthCardType.Soldier);
        player.AddGrowthCard(GrowthCardType.Soldier);
        player.CycleGrowthCards();
        player.RemoveGrowthCard(GrowthCardType.Soldier);
        player.RemoveGrowthCard(GrowthCardType.Soldier);
        player.RemoveGrowthCard(GrowthCardType.Soldier);

        Assert.False(player.ScoreManager.HasLargestArmy);

        // Act
        playerManager.UpdateLargestArmyPlayer();

        // Assert
        Assert.True(player.ScoreManager.HasLargestArmy);
    }

    [Fact]
    public void UpdateLargestArmyPlayer_Should_NotGiveLargestArmyCard_WhenPlayerDoesNotHaveLargestArmy()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;
        player.AddGrowthCard(GrowthCardType.Soldier);
        player.CycleGrowthCards();
        player.RemoveGrowthCard(GrowthCardType.Soldier);

        Assert.False(player.ScoreManager.HasLargestArmy);

        // Act
        playerManager.UpdateLargestArmyPlayer();

        // Assert
        Assert.False(player.ScoreManager.HasLargestArmy);
        Assert.Null(playerManager.LargestArmyPlayer);
    }

    [Fact]
    public void UpdateLargestArmyPlayer_Should_NotGiveLargestArmyCard_WhenPlayerDoesNotHaveMinimumSoldiers()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;
        player.AddGrowthCard(GrowthCardType.Soldier);
        player.CycleGrowthCards();
        player.RemoveGrowthCard(GrowthCardType.Soldier);

        Assert.False(player.ScoreManager.HasLargestArmy);

        // Act
        playerManager.UpdateLargestArmyPlayer();

        // Assert
        Assert.False(player.ScoreManager.HasLargestArmy);
        Assert.Null(playerManager.LargestArmyPlayer);
    }

    [Fact]
    public void UpdateLargestArmyPlayer_Should_RemoveLargestArmyCard_WhenPlayerNoLongerHasLargestArmy()
    {
        // Arrange
        var playerManager = new PlayerManager(4);
        var player = playerManager.CurrentPlayer;
        var player2 = playerManager.Players.First(p => p.Colour != player.Colour);
        player.AddGrowthCard(GrowthCardType.Soldier);
        player.AddGrowthCard(GrowthCardType.Soldier);
        player.AddGrowthCard(GrowthCardType.Soldier);
        player.CycleGrowthCards();
        player.RemoveGrowthCard(GrowthCardType.Soldier);
        player.RemoveGrowthCard(GrowthCardType.Soldier);
        player.RemoveGrowthCard(GrowthCardType.Soldier);

        playerManager.UpdateLargestArmyPlayer();

        Assert.True(player.ScoreManager.HasLargestArmy);
        Assert.False(player2.ScoreManager.HasLargestArmy);

        player2.AddGrowthCard(GrowthCardType.Soldier);
        player2.AddGrowthCard(GrowthCardType.Soldier);
        player2.AddGrowthCard(GrowthCardType.Soldier);
        player2.AddGrowthCard(GrowthCardType.Soldier);
        player2.CycleGrowthCards();
        player2.RemoveGrowthCard(GrowthCardType.Soldier);
        player2.RemoveGrowthCard(GrowthCardType.Soldier);
        player2.RemoveGrowthCard(GrowthCardType.Soldier);
        player2.RemoveGrowthCard(GrowthCardType.Soldier);

        // Act
        playerManager.UpdateLargestArmyPlayer();

        // Assert
        Assert.False(player.ScoreManager.HasLargestArmy);
        Assert.True(player2.ScoreManager.HasLargestArmy);
    }
}
