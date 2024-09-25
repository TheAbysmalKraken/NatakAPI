using Natak.Domain.Enums;
using Natak.Domain.Factories;
using Natak.Domain.Managers;

namespace Natak.Domain.UnitTests;

public sealed class GameTests
{
    [Fact]
    public void PlaceTown_Should_ReturnFailure_WhenInIncorrectState()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableTownLocations().Value;

        // Act
        var result = game.PlaceTown(availableLocations.First());

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaceTown_Should_ReturnFailure_WhenPlayerHasNoTownPieces()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            RemovePlayersPieces = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableTownLocations().Value;

        // Act
        var result = game.PlaceTown(availableLocations.First());

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaceTown_Should_RemoveTownPiece_WhenSuccessful()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableTownLocations().Value;
        var existingTowns = game.CurrentPlayer.PieceManager.Towns;

        // Act
        var result = game.PlaceTown(availableLocations.First());

        // Assert
        var newTowns = game.CurrentPlayer.PieceManager.Towns;
        Assert.True(result.IsSuccess);
        Assert.Equal(existingTowns - 1, newTowns);
    }

    [Fact]
    public void PlaceTown_Should_AddVillagePiece_WhenSuccessful()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableTownLocations().Value;
        var existingVillages = game.CurrentPlayer.PieceManager.Villages;

        // Act
        var result = game.PlaceTown(availableLocations.First());

        // Assert
        var newVillages = game.CurrentPlayer.PieceManager.Villages;
        Assert.True(result.IsSuccess);
        Assert.Equal(existingVillages + 1, newVillages);
    }

    [Fact]
    public void PlaceTown_Should_PlaceTown()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableTownLocations().Value;
        var point = availableLocations.First();

        // Act
        var result = game.PlaceTown(point);

        // Assert
        var town = game.Board.GetHouse(point);
        Assert.True(result.IsSuccess);
        Assert.NotNull(town);
        Assert.Equal(BuildingType.Town, town.Type);
    }

    [Fact]
    public void PlaceTown_Should_SetWinner_IfPlayerNeedsOnePoint()
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
        var availableLocations = game.GetAvailableTownLocations().Value;

        // Act
        var result = game.PlaceTown(availableLocations.First());

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
    public void PlaceRoad_Should_FinishRoaming_WhenPlayerHasPlacedTwoRoads()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            GivePlayersGrowthCards = true,
            HasRolled = false
        };
        var game = GameFactory.Create(gameOptions);
        game.PlayRoamingCard();

        Assert.Equal(GameState.Roaming, game.CurrentState);

        var availableLocations = game.GetAvailableRoadLocations().Value;
        var availableRoad = availableLocations.First();
        var availableRoad2 = availableLocations.Last();

        game.PlaceRoad(availableRoad.FirstPoint, availableRoad.SecondPoint);

        Assert.Equal(GameState.Roaming, game.CurrentState);

        // Act
        var result = game.PlaceRoad(availableRoad2.FirstPoint, availableRoad2.SecondPoint);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(GameState.BeforeRoll, game.CurrentState);
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
    public void PlaceVillage_Should_ReturnFailure_WhenInIncorrectState()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            PrepareVillagePlacement = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableVillageLocations().Value;

        // Act
        var result = game.PlaceVillage(availableLocations.First());

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaceVillage_Should_ReturnFailure_WhenPlayerHasNoVillagePieces()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            RemovePlayersPieces = true,
            PrepareVillagePlacement = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableVillageLocations().Value;

        // Act
        var result = game.PlaceVillage(availableLocations.First());

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaceVillage_Should_RemoveVillagePiece_WhenSuccessful()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true,
            PrepareVillagePlacement = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableVillageLocations().Value;
        var existingVillages = game.CurrentPlayer.PieceManager.Villages;

        // Act
        var result = game.PlaceVillage(availableLocations.First());

        // Assert
        var newVillages = game.CurrentPlayer.PieceManager.Villages;
        Assert.True(result.IsSuccess);
        Assert.Equal(existingVillages - 1, newVillages);
    }

    [Fact]
    public void PlaceVillage_Should_PlaceVillage()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true,
            PrepareVillagePlacement = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableVillageLocations().Value;
        var point = availableLocations.First();

        // Act
        var result = game.PlaceVillage(point);

        // Assert
        var village = game.Board.GetHouse(point);
        Assert.True(result.IsSuccess);
        Assert.NotNull(village);
        Assert.Equal(BuildingType.Village, village.Type);
    }

    [Fact]
    public void PlaceVillage_Should_SetWinner_IfPlayerNeedsOnePoint()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true,
            PlayersVisiblePoints = 9,
            PlayersHiddenPoints = 0,
            PrepareVillagePlacement = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableVillageLocations().Value;

        // Act
        var result = game.PlaceVillage(availableLocations.First());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(game.PlayerManager.WinningPlayer);
        Assert.Equal(game.PlayerManager.WinningPlayer.Colour, game.CurrentPlayerColour);
    }

    [Fact]
    public void PlaceVillage_Should_GivePlayerPort_WhenPlacedOnPort()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true,
            PrepareVillagePlacement = true
        };
        var game = GameFactory.Create(gameOptions);
        var availableLocations = game.GetAvailableVillageLocations().Value;

        // Act
        var result = game.PlaceVillage(availableLocations.First());

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
            { ResourceType.Clay, 10 },
            { ResourceType.Animal, 5 }
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
            { ResourceType.Clay, 10 },
            { ResourceType.Animal, 5 }
        };

        // Act
        var result = game.DiscardResources(player, resources);

        // Assert
        var newPlayerResources = player.ResourceCardManager.Cards;
        var newBankResources = game.BankManager.ResourceCards;
        Assert.True(result.IsSuccess);
        Assert.Equal(0, game.CurrentPlayer.CardsToDiscard);
        Assert.Equal(playerResources[ResourceType.Wood] - resources[ResourceType.Wood], newPlayerResources[ResourceType.Wood]);
        Assert.Equal(playerResources[ResourceType.Clay] - resources[ResourceType.Clay], newPlayerResources[ResourceType.Clay]);
        Assert.Equal(playerResources[ResourceType.Animal] - resources[ResourceType.Animal], newPlayerResources[ResourceType.Animal]);
        Assert.Equal(bankResources[ResourceType.Wood] + resources[ResourceType.Wood], newBankResources[ResourceType.Wood]);
        Assert.Equal(bankResources[ResourceType.Clay] + resources[ResourceType.Clay], newBankResources[ResourceType.Clay]);
        Assert.Equal(bankResources[ResourceType.Animal] + resources[ResourceType.Animal], newBankResources[ResourceType.Animal]);
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
            { ResourceType.Clay, 10 },
            { ResourceType.Animal, 5 }
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
            { ResourceType.Clay, 10 },
            { ResourceType.Animal, 5 }
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

    [Fact]
    public void EndTurn_Should_ReturnFailure_WhenInIncorrectState()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.EndTurn();

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void EndTurn_Should_MoveToNextPlayersTurn()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            HasRolled = true
        };
        var game = GameFactory.Create(gameOptions);
        var currentPlayer = game.CurrentPlayer;

        // Act
        var result = game.EndTurn();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(currentPlayer, game.CurrentPlayer);
    }

    [Fact]
    public void EndTurn_Should_ResetGrowthCardPlayed()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            HasRolled = true,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);
        game.PlayGathererCard(ResourceType.Wood);

        Assert.True(game.GrowthCardPlayed);

        // Act
        var result = game.EndTurn();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(game.GrowthCardPlayed);
    }

    [Fact]
    public void EndTurn_Should_EndTradeOffer()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersResources = true,
            HasRolled = true
        };
        var game = GameFactory.Create(gameOptions);
        game.MakeTradeOffer(
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Wood, 1 }
            },
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Wood, 1 }
            });

        Assert.True(game.TradeManager.TradeOffer.IsActive);

        // Act
        var result = game.EndTurn();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(game.TradeManager.TradeOffer.IsActive);
    }

    [Fact]
    public void EndTurn_Should_ChangeToMainGameState_WhenLeavingSetupState()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsEndOfSetup = true
        };
        var game = GameFactory.Create(gameOptions);

        Assert.IsAssignableFrom<SetupStateManager>(game.StateManager);
        Assert.True(game.PlayerManager.IsSetup);

        // Act
        var result = game.EndTurn();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsAssignableFrom<GameStateManager>(game.StateManager);
        Assert.False(game.PlayerManager.IsSetup);
    }

    [Fact]
    public void PlaySoldierCard_Should_ReturnFailure_WhenInIncorrectState()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = true,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlaySoldierCard();

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaySoldierCard_Should_ReturnFailure_WhenPlayerHasNoSoldierCards()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = false
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlaySoldierCard();

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaySoldierCard_Should_ReturnFailure_WhenPlayerHasAlreadyPlayedGrowthCard()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);
        game.PlayGathererCard(ResourceType.Wood);

        // Act
        var result = game.PlaySoldierCard();

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlaySoldierCard_Should_StopMoreGrowthCardsBeingPlayedThisTurn()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlaySoldierCard();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(game.GrowthCardPlayed);
    }

    [Fact]
    public void PlaySoldierCard_Should_SetWinner_IfPlayerNeedsOnePoint_AndGetsLargestArmy()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = true,
            PlayersVisiblePoints = 9,
            PlayersHiddenPoints = 0,
            PrepareLargestArmy = true
        };
        var game = GameFactory.Create(gameOptions);

        Assert.Null(game.PlayerManager.WinningPlayer);

        // Act
        var result = game.PlaySoldierCard();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(game.PlayerManager.WinningPlayer);
        Assert.Equal(game.PlayerManager.WinningPlayer.Colour, game.CurrentPlayerColour);
        Assert.Equal(GameState.Finish, game.CurrentState);
    }

    [Fact]
    public void PlayGathererCard_Should_ReturnFailure_WhenInIncorrectState()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = true,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlayGathererCard(ResourceType.Wood);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlayGathererCard_Should_ReturnFailure_WhenPlayerHasNoGathererCards()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = false
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlayGathererCard(ResourceType.Wood);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlayGathererCard_Should_ReturnFailure_WhenPlayerHasAlreadyPlayedGrowthCard()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);
        game.PlayGathererCard(ResourceType.Wood);

        // Act
        var result = game.PlayGathererCard(ResourceType.Wood);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlayGathererCard_Should_StopMoreGrowthCardsBeingPlayedThisTurn()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlayGathererCard(ResourceType.Wood);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(game.GrowthCardPlayed);
    }

    [Fact]
    public void PlayGathererCard_Should_GivePlayerResources()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);
        var player = game.CurrentPlayer;
        var initialPlayerWood = player.CountResourceCard(ResourceType.Wood);
        var otherPlayer = game.PlayerManager.Players.First(p => p.Colour != player.Colour);
        otherPlayer.AddResourceCard(ResourceType.Wood, 5);
        var allOtherPlayersWood = game.PlayerManager.Players
            .Where(p => p.Colour != player.Colour)
            .Sum(p => p.CountResourceCard(ResourceType.Wood));

        // Act
        var result = game.PlayGathererCard(ResourceType.Wood);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(initialPlayerWood + allOtherPlayersWood, player.CountResourceCard(ResourceType.Wood));
        Assert.Equal(0, otherPlayer.CountResourceCard(ResourceType.Wood));
    }

    [Fact]
    public void PlayRoamingCard_Should_ReturnFailure_WhenInIncorrectState()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = true,
            GivePlayersGrowthCards = true,
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlayRoamingCard();

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlayRoamingCard_Should_ReturnFailure_WhenPlayerHasNoRoamingCards()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = false
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlayRoamingCard();

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlayRoamingCard_Should_ReturnFailure_WhenPlayerHasAlreadyPlayedGrowthCard()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);
        game.PlayGathererCard(ResourceType.Wood);

        // Act
        var result = game.PlayRoamingCard();

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlayRoamingCard_Should_StopMoreGrowthCardsBeingPlayedThisTurn()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlayRoamingCard();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(game.GrowthCardPlayed);
    }

    [Fact]
    public void PlayWealthCard_Should_ReturnFailure_WhenInIncorrectState()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = true,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlayWealthCard(ResourceType.Wood, ResourceType.Clay);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlayWealthCard_Should_ReturnFailure_WhenPlayerHasNoWealthCards()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = false
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlayWealthCard(ResourceType.Wood, ResourceType.Clay);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlayWealthCard_Should_ReturnFailure_WhenPlayerHasAlreadyPlayedGrowthCard()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);
        game.PlayWealthCard(ResourceType.Wood, ResourceType.Clay);

        // Act
        var result = game.PlayWealthCard(ResourceType.Wood, ResourceType.Clay);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void PlayWealthCard_Should_StopMoreGrowthCardsBeingPlayedThisTurn()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.PlayWealthCard(ResourceType.Wood, ResourceType.Clay);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(game.GrowthCardPlayed);
    }

    [Fact]
    public void PlayWealthCard_Should_GivePlayerResources()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
            GivePlayersGrowthCards = true
        };
        var game = GameFactory.Create(gameOptions);
        var player = game.CurrentPlayer;
        var initialPlayerWood = player.CountResourceCard(ResourceType.Wood);
        var initialPlayerClay = player.CountResourceCard(ResourceType.Clay);

        // Act
        var result = game.PlayWealthCard(ResourceType.Wood, ResourceType.Clay);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(initialPlayerWood + 1, player.CountResourceCard(ResourceType.Wood));
        Assert.Equal(initialPlayerClay + 1, player.CountResourceCard(ResourceType.Clay));
    }

    [Fact]
    public void RollDice_Should_ReturnFailure_WhenInIncorrectState()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = true
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.RollDice();

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void RollDice_Should_SetLastRoll()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false
        };
        var game = GameFactory.Create(gameOptions);

        // Act
        var result = game.RollDice();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(game.LastRoll);
    }

    [Fact]
    public void DistributeResources_Should_GiveResourcesToPlayers_WhoHaveHousesOnLastRolledNumber()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
        };
        var game = GameFactory.Create(gameOptions);

        game.RollDice();

        var initialPlayerResources = game.PlayerManager.Players
            .ToDictionary(
                p => p.Colour,
                p => p.ResourceCardManager.Cards.ToDictionary());

        var lastRollTotal = game.LastRoll.Total;
        var tilesWithNumber = game.Board.GetPointsOfTilesWithActivationNumber(lastRollTotal);

        var playerResourcesGained = new Dictionary<PlayerColour, Dictionary<ResourceType, int>>();

        foreach (var point in tilesWithNumber)
        {
            var housesActivated = game.Board.GetHousesOnTile(point);

            foreach (var house in housesActivated)
            {
                var resource = game.Board.GetTile(point)!.Type;

                var resourceCount = house.Type switch
                {
                    BuildingType.Village => 1,
                    BuildingType.Town => 2,
                    _ => 0
                };

                if (!playerResourcesGained.ContainsKey(house.Colour))
                {
                    playerResourcesGained[house.Colour] = [];
                }

                if (playerResourcesGained[house.Colour].ContainsKey(resource))
                {
                    playerResourcesGained[house.Colour][resource] = resourceCount;
                }
            }
        }

        // Act
        game.DistributeResources();

        // Assert
        foreach (var playerColour in playerResourcesGained.Keys)
        {
            var gainedResources = playerResourcesGained[playerColour];
            var player = game.PlayerManager.GetPlayer(playerColour);

            var newResources = player!.ResourceCardManager.Cards;

            foreach (var gainedResource in gainedResources)
            {
                Assert.Equal(initialPlayerResources[player.Colour][gainedResource.Key] + gainedResource.Value, newResources[gainedResource.Key]);
            }
        }
    }

    [Fact]
    public void DistributeResources_Should_NotGiveResources_ToHousesOnTileWithThief()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false,
        };
        var game = GameFactory.Create(gameOptions);

        game.RollDice();

        var initialPlayerResources = game.PlayerManager.Players
            .ToDictionary(
                p => p.Colour,
                p => p.ResourceCardManager.Cards.ToDictionary());

        var lastRollTotal = game.LastRoll.Total;
        var tilesWithNumber = game.Board.GetPointsOfTilesWithActivationNumber(lastRollTotal);

        if (tilesWithNumber.Count == 0)
        {
            return;
        }

        game.Board.MoveThiefToPoint(tilesWithNumber.First());

        var thiefPoint = game.Board.ThiefPosition;

        var playerResourcesGained = new Dictionary<PlayerColour, Dictionary<ResourceType, int>>();

        foreach (var point in tilesWithNumber)
        {
            if (point == thiefPoint)
            {
                continue;
            }

            var housesActivated = game.Board.GetHousesOnTile(point);

            foreach (var house in housesActivated)
            {
                var resource = game.Board.GetTile(point)!.Type;

                var resourceCount = house.Type switch
                {
                    BuildingType.Village => 1,
                    BuildingType.Town => 2,
                    _ => 0
                };

                if (!playerResourcesGained.ContainsKey(house.Colour))
                {
                    playerResourcesGained[house.Colour] = [];
                }

                if (playerResourcesGained[house.Colour].ContainsKey(resource))
                {
                    playerResourcesGained[house.Colour][resource] = resourceCount;
                }
            }
        }

        // Act
        game.DistributeResources();

        // Assert
        foreach (var playerColour in playerResourcesGained.Keys)
        {
            var gainedResources = playerResourcesGained[playerColour];
            var player = game.PlayerManager.GetPlayer(playerColour);

            var newResources = player!.ResourceCardManager.Cards;

            foreach (var gainedResource in gainedResources)
            {
                Assert.Equal(initialPlayerResources[player.Colour][gainedResource.Key] + gainedResource.Value, newResources[gainedResource.Key]);
            }
        }
    }
}
