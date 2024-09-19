using Catan.Domain.Enums;
using Catan.Domain.Factories;

namespace Catan.Domain.UnitTests;

public sealed class GameTests
{
    [Fact]
    public void PlaceCity_Should_ReturnFailure_WhenInIncorrectState()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableCityLocations().Value;

        // Act
        var result = game.PlaceCity(availableLocations.First());

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaceCity_Should_ReturnFailure_WhenPlayerHasNoCityPieces()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            RemovePlayersPieces = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableCityLocations().Value;

        // Act
        var result = game.PlaceCity(availableLocations.First());

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaceCity_Should_RemoveCityPiece_WhenSuccessful()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableCityLocations().Value;
        var existingCities = game.CurrentPlayer.PieceManager.Cities;

        // Act
        var result = game.PlaceCity(availableLocations.First());

        // Assert
        var newCities = game.CurrentPlayer.PieceManager.Cities;
        Assert.True(result.IsSuccess);
        Assert.Equal(existingCities - 1, newCities);
    }

    [Fact]
    public void PlaceCity_Should_AddSettlementPiece_WhenSuccessful()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableCityLocations().Value;
        var existingSettlements = game.CurrentPlayer.PieceManager.Settlements;

        // Act
        var result = game.PlaceCity(availableLocations.First());

        // Assert
        var newSettlements = game.CurrentPlayer.PieceManager.Settlements;
        Assert.True(result.IsSuccess);
        Assert.Equal(existingSettlements + 1, newSettlements);
    }

    [Fact]
    public void PlaceCity_Should_PlaceCity()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableCityLocations().Value;
        var point = availableLocations.First();

        // Act
        var result = game.PlaceCity(point);

        // Assert
        var city = game.Board.GetHouse(point);
        Assert.True(result.IsSuccess);
        Assert.NotNull(city);
        Assert.Equal(BuildingType.City, city.Type);
    }

    [Fact]
    public void PlaceCity_Should_SetWinner_IfPlayerNeedsOnePoint()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true,
            PlayersVisiblePoints = 9,
            PlayersHiddenPoints = 0
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableCityLocations().Value;

        // Act
        var result = game.PlaceCity(availableLocations.First());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(game.PlayerManager.WinningPlayer);
        Assert.Equal(game.PlayerManager.WinningPlayer.Colour, game.CurrentPlayerColour);
    }

    [Fact]
    public void PlaceRoad_Should_ReturnFailure_WhenInIncorrectState()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableRoad = game.GetAvailableRoadLocations().Value.First();

        // Act
        var result = game.PlaceRoad(availableRoad.FirstPoint, availableRoad.SecondPoint);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaceRoad_Should_ReturnFailure_WhenPlayerHasNoRoadPieces()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            RemovePlayersPieces = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableRoad = game.GetAvailableRoadLocations().Value.First();

        // Act
        var result = game.PlaceRoad(availableRoad.FirstPoint, availableRoad.SecondPoint);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaceRoad_Should_RemoveRoadPiece_WhenSuccessful()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableRoad = game.GetAvailableRoadLocations().Value.First();
        var existingRoads = game.CurrentPlayer.PieceManager.Roads;

        // Act
        var result = game.PlaceRoad(availableRoad.FirstPoint, availableRoad.SecondPoint);

        // Assert
        var newRoads = game.CurrentPlayer.PieceManager.Roads;
        Assert.True(result.IsSuccess);
        Assert.Equal(existingRoads - 1, newRoads);
    }

    [Fact]
    public void PlaceRoad_Should_PlaceRoad()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableRoadLocations().Value;
        var availableRoad = availableLocations.First();

        // Act
        var result = game.PlaceRoad(availableRoad.FirstPoint, availableRoad.SecondPoint);

        // Assert
        var placedRoad = game.Board.GetRoadAtPoints(availableRoad.FirstPoint, availableRoad.SecondPoint);
        Assert.True(result.IsSuccess);
        Assert.NotNull(placedRoad);
        Assert.Equal(BuildingType.Road, placedRoad.Type);
    }

    [Fact]
    public void PlaceRoad_Should_SetWinner_IfPlayerNeedsOnePoint_AndGetsLongestRoad()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true,
            PlayersVisiblePoints = 8,
            PlayersHiddenPoints = 0,
            PrepareLongestRoad = true
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlaceRoad(new(1, 3), new(1, 4));

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(game.PlayerManager.WinningPlayer);
        Assert.Equal(game.PlayerManager.WinningPlayer.Colour, game.CurrentPlayerColour);
        Assert.Equal(GameState.Finish, game.CurrentState);
    }

    [Fact]
    public void PlaceSettlement_Should_ReturnFailure_WhenInIncorrectState()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            PrepareSettlementPlacement = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableSettlementLocations().Value;

        // Act
        var result = game.PlaceSettlement(availableLocations.First());

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaceSettlement_Should_ReturnFailure_WhenPlayerHasNoSettlementPieces()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            RemovePlayersPieces = true,
            PrepareSettlementPlacement = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableSettlementLocations().Value;

        // Act
        var result = game.PlaceSettlement(availableLocations.First());

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaceSettlement_Should_RemoveSettlementPiece_WhenSuccessful()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true,
            PrepareSettlementPlacement = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableSettlementLocations().Value;
        var existingSettlements = game.CurrentPlayer.PieceManager.Settlements;

        // Act
        var result = game.PlaceSettlement(availableLocations.First());

        // Assert
        var newSettlements = game.CurrentPlayer.PieceManager.Settlements;
        Assert.True(result.IsSuccess);
        Assert.Equal(existingSettlements - 1, newSettlements);
    }

    [Fact]
    public void PlaceSettlement_Should_PlaceSettlement()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true,
            PrepareSettlementPlacement = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableSettlementLocations().Value;
        var point = availableLocations.First();

        // Act
        var result = game.PlaceSettlement(point);

        // Assert
        var settlement = game.Board.GetHouse(point);
        Assert.True(result.IsSuccess);
        Assert.NotNull(settlement);
        Assert.Equal(BuildingType.Settlement, settlement.Type);
    }

    [Fact]
    public void PlaceSettlement_Should_SetWinner_IfPlayerNeedsOnePoint()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true,
            PlayersVisiblePoints = 9,
            PlayersHiddenPoints = 0,
            PrepareSettlementPlacement = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableSettlementLocations().Value;

        // Act
        var result = game.PlaceSettlement(availableLocations.First());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(game.PlayerManager.WinningPlayer);
        Assert.Equal(game.PlayerManager.WinningPlayer.Colour, game.CurrentPlayerColour);
    }

    [Fact]
    public void PlaceSettlement_Should_GivePlayerPort_WhenPlacedOnPort()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true,
            PrepareSettlementPlacement = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableSettlementLocations().Value;

        // Act
        var result = game.PlaceSettlement(availableLocations.First());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(game.CurrentPlayer.Ports);
    }

    [Fact]
    public void DiscardResources_Should_ReturnFailure_WhenPlayerDoesNotNeedToDiscard()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true
        };
        var game = GameFactory.Create(gameOptions);
        var resources = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wood, 10 },
            { ResourceType.Brick, 10 },
            { ResourceType.Sheep, 5 }
        };

        // Act
        var result = game.DiscardResources(game.CurrentPlayer, resources);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void DiscardResources_Should_ReturnFailure_WhenDiscardingIncorrectNumberOfResources()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolledSeven = true
        };
        var game = GameFactory.Create(gameOptions);
        var player = game.CurrentPlayer;
        var playerResources = player.ResourceCardManager.Cards;

        // Act
        var result = game.DiscardResources(player, playerResources);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void DiscardResources_Should_DiscardPlayerResourcesToBank()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolledSeven = true
        };
        var game = GameFactory.Create(gameOptions);
        var player = game.CurrentPlayer;
        var playerResources = player.ResourceCardManager.Cards.ToDictionary();
        var bankResources = game.BankManager.ResourceCards.ToDictionary();
        var resources = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wood, 10 },
            { ResourceType.Brick, 10 },
            { ResourceType.Sheep, 5 }
        };

        // Act
        var result = game.DiscardResources(player, resources);

        // Assert
        var newPlayerResources = player.ResourceCardManager.Cards;
        var newBankResources = game.BankManager.ResourceCards;
        Assert.True(result.IsSuccess);
        Assert.Equal(0, game.CurrentPlayer.CardsToDiscard);
        Assert.Equal(playerResources[ResourceType.Wood] - resources[ResourceType.Wood], newPlayerResources[ResourceType.Wood]);
        Assert.Equal(playerResources[ResourceType.Brick] - resources[ResourceType.Brick], newPlayerResources[ResourceType.Brick]);
        Assert.Equal(playerResources[ResourceType.Sheep] - resources[ResourceType.Sheep], newPlayerResources[ResourceType.Sheep]);
        Assert.Equal(bankResources[ResourceType.Wood] + resources[ResourceType.Wood], newBankResources[ResourceType.Wood]);
        Assert.Equal(bankResources[ResourceType.Brick] + resources[ResourceType.Brick], newBankResources[ResourceType.Brick]);
        Assert.Equal(bankResources[ResourceType.Sheep] + resources[ResourceType.Sheep], newBankResources[ResourceType.Sheep]);
    }

    [Fact]
    public void DiscardResources_Should_NotMoveState_WhenMorePlayersHaveToDiscard()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolledSeven = true
        };
        var game = GameFactory.Create(gameOptions);
        var player = game.CurrentPlayer;
        var resources = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wood, 10 },
            { ResourceType.Brick, 10 },
            { ResourceType.Sheep, 5 }
        };

        // Act
        var result = game.DiscardResources(player, resources);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(GameState.DiscardResources, game.CurrentState);
        Assert.True(game.PlayerManager.PlayersNeedToDiscard);
    }

    [Fact]
    public void DiscardResources_Should_MoveState_WhenAllPlayersHaveDiscarded()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolledSeven = true
        };
        var game = GameFactory.Create(gameOptions);
        var resources = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wood, 10 },
            { ResourceType.Brick, 10 },
            { ResourceType.Sheep, 5 }
        };

        // Act
        var result = Result.Success();
        foreach (var player in game.PlayerManager.Players)
        {
            result = game.DiscardResources(player, resources);
        }

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(GameState.DiscardResources, game.CurrentState);
        Assert.False(game.PlayerManager.PlayersNeedToDiscard);
    }
}
