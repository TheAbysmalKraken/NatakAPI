using Natak.Domain.Enums;
using Natak.Domain.Errors;
using Natak.Domain.Managers;

namespace Natak.Domain;

public sealed class Game
{
    private const int MAX_ROAD_BUILDING_ROADS = 2;

    private readonly Stack<DiceRoll> diceRolls = [];
    private int roamingRoadsLeftToPlace = 0;

    public Game(int playerCount)
    {
        TradeManager = new PlayerTradeManager(playerCount);
        PlayerManager = new PlayerManager(playerCount);

        diceRolls.Push(DiceRoller.RollDice(2, 6));
    }

    public string Id { get; init; } = Guid.NewGuid().ToString();

    public Board Board { get; init; } = new();

    public StateManager StateManager { get; private set; } = new SetupStateManager();

    public PlayerTradeManager TradeManager { get; init; }

    public BankTradeManager BankManager { get; init; } = new();

    public PlayerManager PlayerManager { get; init; }

    public bool GrowthCardPlayed { get; private set; }

    public DiceRoll LastRoll => diceRolls.First();

    public Player CurrentPlayer => PlayerManager.CurrentPlayer;

    public PlayerColour CurrentPlayerColour => PlayerManager.CurrentPlayerColour;

    public GameState CurrentState => StateManager.CurrentState;

    public bool IsSetup => PlayerManager.IsSetup;

    public Result MoveState(ActionType actionType)
    {
        return StateManager.MoveState(actionType);
    }

    public Result<List<Point>> GetAvailableTownLocations()
    {
        return Board.GetAvailableTownLocations(CurrentPlayerColour);
    }

    public Result BuyTown()
    {
        return PurchaseHelper.BuyTown(CurrentPlayer, BankManager);
    }

    public Result PlaceTown(
        Point point)
    {
        var moveStateResult = MoveState(ActionType.BuildTown);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var playerPieceResult = CurrentPlayer.RemovePiece(BuildingType.Town);

        if (playerPieceResult.IsFailure)
        {
            return playerPieceResult;
        }

        CurrentPlayer.AddPiece(BuildingType.Village);

        var placeResult = Board.UpgradeHouse(point, CurrentPlayerColour);

        if (placeResult.IsFailure)
        {
            return placeResult;
        }

        CheckForWinner();

        return Result.Success();
    }

    public Result<List<Road>> GetAvailableRoadLocations()
    {
        return Board.GetAvailableRoadLocations(CurrentPlayerColour, IsSetup);
    }

    public Result BuyRoad()
    {
        return PurchaseHelper.BuyRoad(CurrentPlayer, BankManager);
    }

    public Result PlaceRoad(Point firstPoint, Point secondPoint)
    {
        var moveStateResult = MoveState(ActionType.BuildRoad);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var playerPieceResult = CurrentPlayer.RemovePiece(BuildingType.Road);

        if (playerPieceResult.IsFailure)
        {
            return playerPieceResult;
        }

        var placeResult = Board.PlaceRoad(firstPoint, secondPoint, CurrentPlayerColour, IsSetup);

        if (placeResult.IsFailure)
        {
            return placeResult;
        }

        if (roamingRoadsLeftToPlace > 0)
        {
            roamingRoadsLeftToPlace--;
        }

        CheckFinishedRoaming();
        CheckForLongestRoad();
        CheckForWinner();

        return Result.Success();
    }

    public Result<List<Point>> GetAvailableVillageLocations()
    {
        return Board.GetAvailableVillageLocations(CurrentPlayerColour, IsSetup);
    }

    public Result BuyVillage()
    {
        return PurchaseHelper.BuyVillage(CurrentPlayer, BankManager);
    }

    public Result PlaceVillage(Point point)
    {
        var moveStateResult = MoveState(ActionType.BuildVillage);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var playerPieceResult = CurrentPlayer.RemovePiece(BuildingType.Village);

        if (playerPieceResult.IsFailure)
        {
            return playerPieceResult;
        }

        var placeHouseResult = Board.PlaceHouse(point, CurrentPlayerColour, IsSetup);

        if (placeHouseResult.IsFailure)
        {
            return placeHouseResult;
        }

        if (PlayerManager.IsSecondRoundOfSetup)
        {
            return GivePlayerResourcesAroundVillage(point);
        }

        UpdatePlayerPorts(point);
        CheckForWinner();

        return Result.Success();
    }

    public Result BuyGrowthCard()
    {
        return PurchaseHelper.BuyGrowthCard(CurrentPlayer, BankManager);
    }

    public void CancelTradeOffer()
    {
        TradeManager.CancelOffer();
    }

    public Player? GetPlayer(PlayerColour playerColour)
    {
        return PlayerManager.GetPlayer(playerColour);
    }

    public Result DiscardResources(
        Player player,
        Dictionary<ResourceType, int> resources)
    {
        if (resources.Values.Sum() != player.CardsToDiscard)
        {
            return Result.Failure(PlayerErrors.IncorrectDiscardCount);
        }

        var discardResult = PurchaseHelper.DiscardResources(player, BankManager, resources);

        if (discardResult.IsFailure)
        {
            return discardResult;
        }

        return UpdateDiscardState();
    }

    public Result EmbargoPlayer(
        PlayerColour embargoingPlayer,
        PlayerColour embargoedPlayer)
    {
        return TradeManager.AddEmbargo(embargoingPlayer, embargoedPlayer);
    }

    public Result EndTurn()
    {
        GrowthCardPlayed = false;
        TradeManager.Inactive();

        var moveStateResult = MoveState(ActionType.EndTurn);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var isSetupBeforeNextPlayer = PlayerManager.IsSetup;

        PlayerManager.NextPlayer();

        if (isSetupBeforeNextPlayer != PlayerManager.IsSetup
            && !PlayerManager.IsSetup)
        {
            StateManager = new GameStateManager();
        }

        return Result.Success();
    }

    public Result MakeTradeOffer(
        Dictionary<ResourceType, int> offer,
        Dictionary<ResourceType, int> request)
    {
        return TradeManager.CreateOffer(CurrentPlayer, offer, request);
    }

    public Result MoveThief(
        Point point)
    {
        var moveStateResult = MoveState(ActionType.MoveThief);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        return Board.MoveThiefToPoint(point);
    }

    public Result RemoveGrowthCardFromCurrentPlayer(
        GrowthCardType growthCardType)
    {
        return CurrentPlayer.RemoveGrowthCard(growthCardType);
    }

    public Result PlaySoldierCard()
    {
        if (GrowthCardPlayed)
        {
            return Result.Failure(PlayerErrors.GrowthCardAlreadyPlayed);
        }

        var moveStateResult = MoveState(ActionType.PlaySoldierCard);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var removeCardResult = CurrentPlayer.RemoveGrowthCard(
            GrowthCardType.Soldier);

        if (removeCardResult.IsFailure)
        {
            return removeCardResult;
        }

        GrowthCardPlayed = true;

        PlayerManager.UpdateLargestArmyPlayer();
        CheckForWinner();

        return Result.Success();
    }

    public Result PlayGathererCard(
        ResourceType resource)
    {
        if (GrowthCardPlayed)
        {
            return Result.Failure(PlayerErrors.GrowthCardAlreadyPlayed);
        }

        var moveStateResult = MoveState(ActionType.PlayGathererCard);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var removeCardResult = CurrentPlayer.RemoveGrowthCard(
            GrowthCardType.Gatherer);

        if (removeCardResult.IsFailure)
        {
            return removeCardResult;
        }

        if (resource == ResourceType.None)
        {
            return Result.Failure(GameErrors.InvalidResourceType);
        }

        PlayerManager.GivePlayerGathererResource(CurrentPlayer, resource);

        GrowthCardPlayed = true;

        return Result.Success();
    }

    public Result PlayRoamingCard()
    {
        if (GrowthCardPlayed)
        {
            return Result.Failure(PlayerErrors.GrowthCardAlreadyPlayed);
        }

        var moveStateResult = MoveState(ActionType.PlayRoamingCard);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var removeCardResult = CurrentPlayer.RemoveGrowthCard(
            GrowthCardType.Roaming);

        if (removeCardResult.IsFailure)
        {
            return removeCardResult;
        }

        GrowthCardPlayed = true;
        roamingRoadsLeftToPlace = MAX_ROAD_BUILDING_ROADS;

        return Result.Success();
    }

    public Result PlayWealthCard(
        ResourceType firstResource,
        ResourceType secondResource)
    {
        if (GrowthCardPlayed)
        {
            return Result.Failure(PlayerErrors.GrowthCardAlreadyPlayed);
        }

        var moveStateResult = MoveState(ActionType.PlayWealthCard);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var removeCardResult = CurrentPlayer.RemoveGrowthCard(
            GrowthCardType.Wealth);

        if (removeCardResult.IsFailure)
        {
            return removeCardResult;
        }

        PurchaseHelper.GetFreeResources(
            CurrentPlayer,
            BankManager, new()
            {
                { firstResource, 1 },
                { secondResource, 1 }
            });

        GrowthCardPlayed = true;

        return Result.Success();
    }

    public Result RemoveEmbargo(
        PlayerColour embargoingPlayer,
        PlayerColour embargoedPlayer)
    {
        return TradeManager.RemoveEmbargo(embargoingPlayer, embargoedPlayer);
    }

    public Result AcceptTradeOffer(PlayerColour playerAcceptingColour)
    {
        var acceptingPlayer = GetPlayer(playerAcceptingColour);

        if (acceptingPlayer is null)
        {
            return Result.Failure(PlayerErrors.NotFound);
        }

        return TradeManager.AcceptOffer(CurrentPlayer, acceptingPlayer);
    }

    public Result RejectTradeOffer(PlayerColour playerRejectingColour)
    {
        return TradeManager.RejectOffer(playerRejectingColour);
    }

    public Result RollDice()
    {
        var newRoll = DiceRoller.RollDice(2, 6);

        if (newRoll.Total == 7)
        {
            var rollSevenResult = RollSeven();

            if (rollSevenResult.IsFailure)
            {
                return rollSevenResult;
            }
        }
        else
        {
            var moveStateResult = MoveState(ActionType.RollDice);

            if (moveStateResult.IsFailure)
            {
                return moveStateResult;
            }
        }

        diceRolls.Push(newRoll);

        return Result.Success();
    }

    public Result DistributeResources()
    {
        var activationNumber = LastRoll.Total;
        var tilePointsWithActivationNumber = Board.GetPointsOfTilesWithActivationNumber(activationNumber);

        foreach (var tilePoint in tilePointsWithActivationNumber)
        {
            DistributeResourcesOnTilePoint(tilePoint);
        }

        return Result.Success();
    }

    public Result StealResourceFromPlayer(PlayerColour playerColour)
    {
        var moveStateResult = MoveState(ActionType.StealResource);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var thiefPosition = Board.ThiefPosition;
        var playerColoursToStealFrom = Board.GetHouseColoursOnTile(thiefPosition);

        if (!playerColoursToStealFrom.Contains(playerColour))
        {
            return Result.Failure(GameErrors.PlayerToStealFromDoesNotHaveHouseOnTile);
        }

        return PlayerManager.StealFromPlayer(CurrentPlayerColour, playerColour);
    }

    public Result TradeWithBank(
        ResourceType offeredResource,
        ResourceType requestedResource)
    {
        return BankManager.Trade(CurrentPlayer, offeredResource, requestedResource);
    }

    private Result GivePlayerResourcesAroundVillage(Point point)
    {
        var tiles = Board.GetTilesSurroundingHouse(point);

        foreach (var tile in tiles)
        {
            var resourceType = tile.Type;

            if (resourceType == ResourceType.None)
            {
                continue;
            }

            PurchaseHelper.GetFreeResources(
            CurrentPlayer,
            BankManager,
            new() { { resourceType, 1 } });
        }

        return Result.Success();
    }

    private Result UpdateDiscardState()
    {
        if (!PlayerManager.PlayersNeedToDiscard)
        {
            var moveStateResult = MoveState(ActionType.AllResourcesDiscarded);

            return moveStateResult;
        }

        return Result.Success();
    }

    private void DistributeResourcesOnTilePoint(Point tilePoint)
    {
        if (tilePoint.Equals(Board.ThiefPosition))
        {
            return;
        }

        var tile = Board.GetTile(tilePoint);

        if (tile is null
        || tile.Type == ResourceType.None)
        {
            return;
        }

        var resourceType = tile.Type;

        var houses = Board.GetHousesOnTile(tilePoint);

        foreach (var house in houses)
        {
            DistributeResourcesForHouse(house, resourceType);
        }
    }

    private void DistributeResourcesForHouse(
        Building house,
        ResourceType resourceType)
    {
        var player = GetPlayer(house.Colour)
                ?? throw new InvalidOperationException("Player not found");

        var isTown = house.Type == BuildingType.Town;

        var resourceCount = isTown ? 2 : 1;

        PurchaseHelper.GetFreeResources(
            player,
            BankManager,
            new() { { resourceType, resourceCount } });
    }

    private Result RollSeven()
    {
        var moveRollSevenStateResult = MoveState(
                ActionType.RollSeven);

        if (moveRollSevenStateResult.IsFailure)
        {
            return moveRollSevenStateResult;
        }

        PlayerManager.CalculateDiscardRequirements();

        return UpdateDiscardState();
    }

    private void CheckForWinner()
    {
        var winner = PlayerManager.WinningPlayer;

        if (winner is null)
        {
            return;
        }

        var moveState = MoveState(ActionType.PlayerHasWon);

        if (moveState.IsFailure)
        {
            throw new InvalidOperationException("Failed to move to player has won state");
        }

        return;
    }

    private void CheckFinishedRoaming()
    {
        if (CurrentState == GameState.Roaming
            && roamingRoadsLeftToPlace == 0)
        {
            MoveState(ActionType.FinishRoaming);
        }
    }

    private void CheckForLongestRoad()
    {
        var longestRoadInfo = Board.LongestRoadInfo;

        var playerColour = longestRoadInfo.Colour;

        PlayerManager.UpdateLongestRoadPlayer(playerColour);
    }

    private void UpdatePlayerPorts(Point point)
    {
        var port = Board.GetPorts().Where(p => p.Point.Equals(point)).FirstOrDefault();

        if (port is null)
        {
            return;
        }

        PlayerManager.GivePort(CurrentPlayerColour, port.Type);
    }
}
