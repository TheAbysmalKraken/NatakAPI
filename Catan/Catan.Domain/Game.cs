using Catan.Domain.Enums;
using Catan.Domain.Errors;
using Catan.Domain.Managers;

namespace Catan.Domain;

public sealed class Game
{
    private readonly Stack<DiceRoll> diceRolls = [];

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

    public bool DevelopmentCardPlayed { get; private set; }

    public DiceRoll LastRoll => diceRolls.First();

    public Player CurrentPlayer => PlayerManager.CurrentPlayer;

    public PlayerColour CurrentPlayerColour => PlayerManager.CurrentPlayerColour;

    public GameState CurrentState => StateManager.CurrentState;

    public bool IsSetup => PlayerManager.IsSetup;

    public Result MoveState(ActionType actionType)
    {
        return StateManager.MoveState(actionType);
    }

    public Result BuyCity()
    {
        return PurchaseHelper.BuyCity(CurrentPlayer, BankManager);
    }

    public Result PlaceCity(
        Point point)
    {
        var moveStateResult = MoveState(ActionType.BuildCity);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var playerPieceResult = CurrentPlayer.RemovePiece(BuildingType.City);

        if (playerPieceResult.IsFailure)
        {
            return playerPieceResult;
        }

        CurrentPlayer.AddPiece(BuildingType.Settlement);

        var placeResult = Board.UpgradeHouse(point, CurrentPlayerColour);

        if (placeResult.IsFailure)
        {
            return placeResult;
        }

        CheckForWinner();

        return Result.Success();
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

        CheckForWinner();

        return Result.Success();
    }

    public Result BuySettlement()
    {
        return PurchaseHelper.BuySettlement(CurrentPlayer, BankManager);
    }

    public Result PlaceSettlement(Point point)
    {
        var moveStateResult = MoveState(ActionType.BuildSettlement);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var playerPieceResult = CurrentPlayer.RemovePiece(BuildingType.Settlement);

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
            return GivePlayerResourcesAroundSettlement(point);
        }

        CheckForWinner();

        return Result.Success();
    }

    public Result BuyDevelopmentCard()
    {
        return PurchaseHelper.BuyDevelopmentCard(CurrentPlayer, BankManager);
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
        DevelopmentCardPlayed = false;
        TradeManager.Inactive();

        var moveStateResult = MoveState(ActionType.EndTurn);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var isSetup = PlayerManager.IsSetup;

        PlayerManager.NextPlayer();

        if (isSetup != PlayerManager.IsSetup)
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

    public Result MoveRobber(
        Point point)
    {
        var moveStateResult = MoveState(ActionType.MoveRobber);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        return Board.MoveRobberToPoint(point);
    }

    public Result RemoveDevelopmentCardFromCurrentPlayer(
        DevelopmentCardType developmentCardType)
    {
        return CurrentPlayer.RemoveDevelopmentCard(developmentCardType);
    }

    public Result PlayKnightCard()
    {
        var moveStateResult = MoveState(ActionType.PlayKnightCard);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        DevelopmentCardPlayed = true;

        CheckForWinner();

        return Result.Success();
    }

    public Result PlayMonopolyCard(
        ResourceType resource)
    {
        var moveStateResult = MoveState(ActionType.PlayMonopolyCard);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        PlayerManager.GivePlayerMonopolyResource(CurrentPlayer, resource);

        DevelopmentCardPlayed = true;

        return Result.Success();
    }

    public Result PlayRoadBuildingCard(
        Point firstRoadFirstPoint,
        Point firstRoadSecondPoint,
        Point secondRoadFirstPoint,
        Point secondRoadSecondPoint)
    {
        var moveStateResult = MoveState(ActionType.PlayRoadBuildingCard);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var buildResult = PlaceRoadBuildingRoads(
            firstRoadFirstPoint,
            firstRoadSecondPoint,
            secondRoadFirstPoint,
            secondRoadSecondPoint);

        if (buildResult.IsFailure)
        {
            return buildResult;
        }

        DevelopmentCardPlayed = true;

        CheckForWinner();

        return Result.Success();
    }

    public Result PlayYearOfPlentyCard(
        ResourceType firstResource,
        ResourceType secondResource)
    {
        var moveStateResult = MoveState(ActionType.PlayYearOfPlentyCard);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        PurchaseHelper.GetFreeResources(
            CurrentPlayer,
            BankManager, new()
            {
                { firstResource, 1 },
                { secondResource, 1 }
            });

        DevelopmentCardPlayed = true;

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

    public Result StealResourceFromPlayer(PlayerColour player)
    {
        var moveStateResult = MoveState(ActionType.StealResource);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        return PlayerManager.StealFromPlayer(CurrentPlayerColour, player);
    }

    public Result TradeWithBank(
        ResourceType offeredResource,
        ResourceType requestedResource)
    {
        return BankManager.Trade(CurrentPlayer, offeredResource, requestedResource);
    }

    private Result GivePlayerResourcesAroundSettlement(Point point)
    {
        var tiles = Board.GetTilesSurroundingHouse(point);

        foreach (var tile in tiles)
        {
            var resourceType = tile.Type;

            if (resourceType == ResourceType.Desert)
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

    private Result PlaceRoadBuildingRoads(
        Point firstRoadFirstPoint,
        Point firstRoadSecondPoint,
        Point secondRoadFirstPoint,
        Point secondRoadSecondPoint)
    {
        var firstRoadResult = PlaceRoad(firstRoadFirstPoint, firstRoadSecondPoint);

        if (firstRoadResult.IsFailure)
        {
            return firstRoadResult;
        }

        return PlaceRoad(secondRoadFirstPoint, secondRoadSecondPoint);
    }

    private void DistributeResourcesOnTilePoint(Point tilePoint)
    {
        if (tilePoint.Equals(Board.RobberPosition))
        {
            return;
        }

        var tile = Board.GetTile(tilePoint);

        if (tile is null
        || tile.Type == ResourceType.Desert)
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

        var isCity = house.Type == BuildingType.City;

        var resourceCount = isCity ? 2 : 1;

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
}
