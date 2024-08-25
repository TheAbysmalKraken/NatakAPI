using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Domain;

public class Game
{
    private const int MIN_PLAYERS = 3;
    private const int MAX_PLAYERS = 4;

    private readonly List<Player> players = [];
    private readonly List<int> rolledDice = [];
    private readonly Dictionary<ResourceType, int> remainingResourceCards = [];
    private readonly Dictionary<DevelopmentCardType, int> remainingDevelopmentCardTotals = [];
    private readonly List<DevelopmentCardType> remainingDevelopmentCards = [];
    private int currentPlayerIndex = 0;
    private int knightsRequiredForLargestArmy;
    private bool developmentCardPlayedThisTurn;
    private TradeOffer tradeOffer;
    private readonly Random random = new();
    private readonly GameStateManager gameStateManager;

    public Game(int numberOfPlayers, int? seed = null)
    {
        if (numberOfPlayers < MIN_PLAYERS || numberOfPlayers > MAX_PLAYERS)
        {
            throw new ArgumentException($"Invalid number of players: '{numberOfPlayers}'");
        }

        if (seed.HasValue)
        {
            random = new(seed.Value);
        }

        Id = Guid.NewGuid().ToString();

        Board = new Board(seed);

        RollDice();
        InitialisePlayers(numberOfPlayers);
        InitialiseResourceCards();
        InitialiseDevelopmentCards();

        knightsRequiredForLargestArmy = 3;
        developmentCardPlayedThisTurn = false;
        gameStateManager = new GameStateManager();
        PlayerCount = numberOfPlayers;
        tradeOffer = TradeOffer.Inactive();
    }

    public string Id { get; init; }

    public int PlayerCount { get; private set; }

    public int? WinnerIndex { get; private set; }

    public List<int> LastRoll => rolledDice;

    public Board Board { get; private set; } = new();

    public Player? LongestRoadPlayer { get; private set; } = null;

    public int? LongestRoadPlayerIndex => SetLongestRoadPlayerIndex();

    public Player? LargestArmyPlayer => players.FirstOrDefault(p => p.HasLargestArmy);

    public int? LargestArmyPlayerIndex => SetLargestArmyPlayerIndex();

    public int DiceTotal => rolledDice.Sum();

    public Player CurrentPlayer => players[currentPlayerIndex];

    public int CurrentPlayerIndex => currentPlayerIndex;

    public bool HasPlayedDevelopmentCardThisTurn => developmentCardPlayedThisTurn;

    public TradeOffer TradeOffer => tradeOffer;

    public GameState CurrentState => gameStateManager.CurrentState;

    public List<ActionType> Actions => gameStateManager.GetValidActions();

    public List<Player> GetPlayers() => players;

    public List<int> GetRolledDice() => rolledDice;

    public Dictionary<ResourceType, int> GetRemainingResourceCards()
        => remainingResourceCards;

    public Dictionary<DevelopmentCardType, int> GetRemainingDevelopmentCardTotals()
        => remainingDevelopmentCardTotals;

    public List<DevelopmentCardType> GetRemainingDevelopmentCards()
        => remainingDevelopmentCards;

    public bool ContainsPlayer(PlayerColour playerColour)
    {
        var intPlayerColour = (int)playerColour;
        return intPlayerColour >= 0 && intPlayerColour < PlayerCount;
    }

    public Result NextPlayer()
    {
        UpdateLargestArmyPlayer();

        developmentCardPlayedThisTurn = false;
        tradeOffer = TradeOffer.Inactive();

        CurrentPlayer.MoveOnHoldDevelopmentCardsToPlayable();

        var handleNextPlayerStateResult = HandleNextPlayerState();

        return handleNextPlayerStateResult;
    }

    public void RollDice()
    {
        rolledDice.Clear();
        rolledDice.AddRange(DiceRoller.RollDice(2, 6));
    }

    public Result RollDiceAndDistributeResourcesToPlayers()
    {
        RollDice();

        var diceTotal = DiceTotal;

        if (diceTotal == 7)
        {
            var moveRollSevenStateResult = gameStateManager.MoveState(
                ActionType.RollSeven);

            if (moveRollSevenStateResult.IsFailure)
            {
                return moveRollSevenStateResult;
            }

            var finishDiscardingResult = TryFinishDiscardingResources();

            return finishDiscardingResult;
        }

        var tilePointsWithActivationNumber = Board.GetPointsOfTilesWithActivationNumber(diceTotal);

        foreach (var point in tilePointsWithActivationNumber)
        {
            if (point.Equals(Board.RobberPosition))
            {
                continue;
            }

            var houses = Board.GetHousesOnTile(point);

            foreach (var house in houses)
            {
                var player = GetPlayerByColour(house.Colour);

                if (player == null)
                {
                    continue;
                }

                var isCity = house.Type == BuildingType.City;

                var tile = Board.GetTile(point);

                if (tile is null
                || tile.Type == ResourceType.Desert
                || tile.Type == ResourceType.None)
                {
                    continue;
                }

                var resourceType = tile.Type;

                if (remainingResourceCards[resourceType] > 0)
                {
                    player.AddResourceCard(resourceType);
                    remainingResourceCards[resourceType]--;

                    if (isCity && remainingResourceCards[resourceType] > 0)
                    {
                        player.AddResourceCard(resourceType);
                        remainingResourceCards[resourceType]--;
                    }
                }
            }
        }

        return Result.Success();
    }

    public void GiveResourcesSurroundingHouse(Point point)
    {
        var tiles = Board.GetTilesSurroundingHouse(point);

        foreach (var tile in tiles)
        {
            var resourceType = tile.Type;

            if (resourceType == ResourceType.None || resourceType == ResourceType.Desert)
            {
                continue;
            }

            if (remainingResourceCards[resourceType] > 0)
            {
                CurrentPlayer.AddResourceCard(resourceType);
                remainingResourceCards[resourceType]--;
            }
        }
    }

    public Result TradeTwoToOne(ResourceType resourceTypeToGive, ResourceType resourceTypeToReceive)
    {
        var tradeStateResult = gameStateManager.MoveState(ActionType.Trade);

        if (tradeStateResult.IsFailure)
        {
            return tradeStateResult;
        }

        var portTypeValid = Enum.TryParse<PortType>(resourceTypeToReceive.ToString(), out var portType);

        if (!portTypeValid)
        {
            throw new InvalidOperationException($"Invalid port type: '{resourceTypeToReceive}'");
        }

        if (!Board.ColourHasPortOfType(CurrentPlayer.Colour, portType))
        {
            return Result.Failure(PlayerErrors.DoesNotOwnPort);
        }

        var tradeResult = CurrentPlayer.TradeTwoToOne(resourceTypeToGive, resourceTypeToReceive);

        return tradeResult;
    }

    public Result TradeThreeToOne(ResourceType resourceTypeToGive, ResourceType resourceTypeToReceive)
    {
        var tradeStateResult = gameStateManager.MoveState(ActionType.Trade);

        if (tradeStateResult.IsFailure)
        {
            return tradeStateResult;
        }

        if (!Board.ColourHasPortOfType(CurrentPlayer.Colour, PortType.ThreeToOne))
        {
            return Result.Failure(PlayerErrors.DoesNotOwnPort);
        }

        var tradeResult = CurrentPlayer.TradeThreeToOne(resourceTypeToGive, resourceTypeToReceive);

        return tradeResult;
    }

    public Result TradeFourToOne(ResourceType resourceTypeToGive, ResourceType resourceTypeToReceive)
    {
        var tradeStateResult = gameStateManager.MoveState(ActionType.Trade);

        if (tradeStateResult.IsFailure)
        {
            return tradeStateResult;
        }

        var tradeResult = CurrentPlayer.TradeFourToOne(resourceTypeToGive, resourceTypeToReceive);

        return tradeResult;
    }

    public Result EmbargoPlayer(PlayerColour colourEmbargoedBy, PlayerColour colourToEmbargo)
    {
        var playerEmbargoedBy = GetPlayerByColour(colourEmbargoedBy);

        if (playerEmbargoedBy == null)
        {
            return Result.Failure(PlayerErrors.NotFound);
        }

        var playerToEmbargo = GetPlayerByColour(colourToEmbargo);

        if (playerToEmbargo == null)
        {
            return Result.Failure(PlayerErrors.NotFound);
        }

        var embargoResult = playerEmbargoedBy.EmbargoPlayer(colourToEmbargo);

        return embargoResult;
    }

    public Result RemovePlayerEmbargo(PlayerColour colourEmbargoedBy, PlayerColour colourToRemoveEmbargoOn)
    {
        var playerEmbargoedBy = GetPlayerByColour(colourEmbargoedBy);

        if (playerEmbargoedBy == null)
        {
            return Result.Failure(PlayerErrors.NotFound);
        }

        var removeEmbargoResult = playerEmbargoedBy.RemoveEmbargo(colourToRemoveEmbargoOn);

        return removeEmbargoResult;
    }

    public Result PlayKnightCard()
    {
        var result = PlayDevelopmentCard(DevelopmentCardType.Knight);

        if (result.IsFailure)
        {
            return result;
        }

        UpdateLargestArmyPlayer();

        SetWinnerIndex();

        return Result.Success();
    }

    public Result PlayYearOfPlentyCard(ResourceType resourceType1, ResourceType resourceType2)
    {
        if (resourceType1 == ResourceType.None || resourceType2 == ResourceType.None)
        {
            return Result.Failure(GameErrors.InvalidResourceType);
        }

        var resourceTotal1 = remainingResourceCards[resourceType1];
        var resourceTotal2 = remainingResourceCards[resourceType2];

        if (resourceTotal1 == 0 || resourceTotal2 == 0)
        {
            return Result.Failure(GameErrors.InsufficientResources);
        }

        if (resourceType1 == resourceType2 && resourceTotal1 < 2)
        {
            return Result.Failure(GameErrors.InsufficientResources);
        }

        var result = PlayDevelopmentCard(DevelopmentCardType.YearOfPlenty);

        if (result.IsFailure)
        {
            return result;
        }

        CurrentPlayer.AddResourceCard(resourceType1);
        CurrentPlayer.AddResourceCard(resourceType2);

        return Result.Success();
    }

    public Result PlayMonopolyCard(ResourceType resourceType)
    {
        if (resourceType == ResourceType.None)
        {
            return Result.Failure(GameErrors.InvalidResourceType);
        }

        var result = PlayDevelopmentCard(DevelopmentCardType.Monopoly);

        if (result.IsFailure)
        {
            return result;
        }

        var playersToStealFrom = players.Where(p => p.Colour != CurrentPlayer.Colour);

        foreach (var player in playersToStealFrom)
        {
            var resourceCount = player.GetResourceCards()[resourceType];

            if (resourceCount > 0)
            {
                player.RemoveResourceCard(resourceType, resourceCount);
                CurrentPlayer.AddResourceCard(resourceType, resourceCount);
            }
        }

        return Result.Success();
    }

    public Result PlayRoadBuildingCard(
        Point point1,
        Point point2,
        Point point3,
        Point point4)
    {
        var canPlaceRoadsResult = Board.CanPlaceRoadBetweenPoints(point1, point2, CurrentPlayer.Colour);

        if (canPlaceRoadsResult.IsFailure)
        {
            return canPlaceRoadsResult;
        }

        var result = PlayDevelopmentCard(DevelopmentCardType.RoadBuilding);

        if (result.IsFailure)
        {
            return result;
        }

        var placeFirstRoadResult = Board.PlaceRoad(point1, point2, CurrentPlayer.Colour);

        if (placeFirstRoadResult.IsFailure)
        {
            throw new Exception("Failed to place first road.");
        }

        var placeSecondRoadResult = Board.PlaceRoad(point3, point4, CurrentPlayer.Colour);

        if (placeSecondRoadResult.IsFailure)
        {
            throw new Exception("Failed to place second road.");
        }

        UpdateLargestRoadPlayer();

        SetWinnerIndex();

        return Result.Success();
    }

    public Result MakeTradeOffer(
        Dictionary<ResourceType, int> offer,
        Dictionary<ResourceType, int> request)
    {
        var canTradeResult = CurrentPlayer.CanMakeTradeOffer(offer);

        if (canTradeResult.IsFailure)
        {
            return canTradeResult;
        }

        tradeOffer = new()
        {
            IsActive = true,
            Offer = offer,
            Request = request
        };

        foreach (var player in players)
        {
            if (player.GetEmbargoedPlayers().Contains(CurrentPlayer.Colour))
            {
                tradeOffer.RejectedBy.Add(player.Colour);
            }
        }

        return Result.Success();
    }

    public Result RejectTradeOffer(PlayerColour playerColour)
    {
        if (playerColour == CurrentPlayer.Colour)
        {
            return Result.Failure(PlayerErrors.CannotTradeWithSelf);
        }

        if (!tradeOffer.IsActive)
        {
            return Result.Failure(GameErrors.TradeOfferNotActive);
        }

        if (tradeOffer.RejectedBy.Contains(playerColour))
        {
            return Result.Failure(PlayerErrors.AlreadyRejectedTrade);
        }

        tradeOffer.RejectedBy.Add(playerColour);

        if (tradeOffer.RejectedBy.Count == PlayerCount - 1)
        {
            tradeOffer = TradeOffer.Inactive();
        }

        return Result.Success();
    }

    public Result AcceptTradeOffer(PlayerColour playerColour)
    {
        var playerToTradeWith = GetPlayerByColour(playerColour);

        if (playerToTradeWith == null)
        {
            return Result.Failure(PlayerErrors.NotFound);
        }

        if (!CurrentPlayer.CanTradeWithPlayer(playerColour)
        || !playerToTradeWith.CanTradeWithPlayer(CurrentPlayer.Colour))
        {
            return Result.Failure(PlayerErrors.Embargoed);
        }

        if (!CurrentPlayer.HasAdequateResourceCardsOfTypes(tradeOffer.Offer)
        || !playerToTradeWith.HasAdequateResourceCardsOfTypes(tradeOffer.Request))
        {
            return Result.Failure(PlayerErrors.MissingResources);
        }

        CurrentPlayer.AddResourceCards(tradeOffer.Request);
        var removeFromCurrentResult = CurrentPlayer.RemoveResourceCards(tradeOffer.Offer);

        if (removeFromCurrentResult.IsFailure)
        {
            throw new Exception("Failed to remove resource cards from current player.");
        }

        playerToTradeWith.AddResourceCards(tradeOffer.Offer);
        var removeFromTradePlayerResult = playerToTradeWith.RemoveResourceCards(tradeOffer.Request);

        if (removeFromTradePlayerResult.IsFailure)
        {
            throw new Exception("Failed to remove resource cards from trade player.");
        }

        return Result.Success();
    }

    public Result BuildRoad(Point point1, Point point2, bool isFree = false)
    {
        if (!isFree && !CurrentPlayer.CanBuyRoad())
        {
            return Result.Failure(PlayerErrors.MissingResources);
        }

        var canPlaceResult = Board.CanPlaceRoadBetweenPoints(point1, point2, CurrentPlayer.Colour);

        if (canPlaceResult.IsFailure)
        {
            return canPlaceResult;
        }

        var moveStateResult = gameStateManager.MoveState(ActionType.BuildRoad);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var result = Board.PlaceRoad(point1, point2, CurrentPlayer.Colour);

        if (result.IsFailure)
        {
            throw new Exception("Failed to place free road.");
        }

        if (!isFree)
        {
            CurrentPlayer.BuyRoad();
        }

        UpdateLargestRoadPlayer();

        SetWinnerIndex();

        return Result.Success();
    }

    public Result BuildSettlement(Point point, bool isFree = false)
    {
        if (!isFree && !CurrentPlayer.CanBuySettlement())
        {
            return Result.Failure(PlayerErrors.MissingResources);
        }

        var canPlaceHouseResult = Board.CanPlaceHouseAtPoint(point, CurrentPlayer.Colour, isFree);

        if (canPlaceHouseResult.IsFailure)
        {
            return canPlaceHouseResult;
        }

        var moveStateResult = gameStateManager.MoveState(ActionType.BuildSettlement);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var result = Board.PlaceHouse(point, CurrentPlayer.Colour, isFree);

        if (result.IsFailure)
        {
            throw new Exception("Failed to place settlement.");
        }

        if (!isFree)
        {
            CurrentPlayer.BuySettlement();
        }

        UpdateLargestRoadPlayer();

        SetWinnerIndex();

        return Result.Success();
    }

    public Result BuildCity(Point point)
    {
        if (!CurrentPlayer.CanBuyCity())
        {
            return Result.Failure(PlayerErrors.MissingResources);
        }

        var canUpgradeResult = Board.CanUpgradeHouseAtPoint(point, CurrentPlayer.Colour);

        if (canUpgradeResult.IsFailure)
        {
            return canUpgradeResult;
        }

        var moveStateResult = gameStateManager.MoveState(ActionType.BuildCity);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var result = Board.UpgradeHouse(point, CurrentPlayer.Colour);

        if (result.IsFailure)
        {
            throw new Exception("Failed to upgrade house to city.");
        }

        CurrentPlayer.BuyCity();

        SetWinnerIndex();

        return Result.Success();
    }

    public Result MoveRobber(Point point)
    {
        var canMoveResult = Board.CanMoveRobberToPoint(point);

        if (canMoveResult.IsFailure)
        {
            return canMoveResult;
        }

        var moveStateResult = gameStateManager.MoveState(ActionType.MoveRobber);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var result = Board.MoveRobberToPoint(point);

        if (result.IsFailure)
        {
            throw new Exception("Failed to move robber to point.");
        }

        return Result.Success();
    }

    public Result StealResourceCard(PlayerColour victimColour)
    {
        var victim = GetPlayerByColour(victimColour);

        if (victim == null)
        {
            return Result.Failure(PlayerErrors.NotFound);
        }

        if (victim == CurrentPlayer)
        {
            return Result.Failure(PlayerErrors.CannotStealFromSelf);
        }

        var stolenCard = victim.RemoveRandomResourceCard();

        if (stolenCard != null)
        {
            CurrentPlayer.AddResourceCard(stolenCard.Value);
        }

        return Result.Success();
    }

    public Result DiscardResources(PlayerColour playerColourDiscarding, Dictionary<ResourceType, int> resourcesToDiscard)
    {
        var player = GetPlayerByColour(playerColourDiscarding);

        if (player == null)
        {
            return Result.Failure(PlayerErrors.NotFound);
        }

        var moveStateResult = gameStateManager.MoveState(ActionType.DiscardResources);

        if (moveStateResult.IsFailure)
        {
            return moveStateResult;
        }

        var result = player.RemoveResourceCards(resourcesToDiscard);

        if (result.IsFailure)
        {
            return result;
        }

        foreach (var type in resourcesToDiscard.Keys)
        {
            remainingResourceCards[type] += resourcesToDiscard[type];
        }

        var canFinishDiscardingResult = TryFinishDiscardingResources();

        if (canFinishDiscardingResult.IsFailure)
        {
            throw new Exception("Failed to finish discarding resources.");
        }

        return Result.Success();
    }

    public Result TryFinishDiscardingResources()
    {
        if (!players.Any(p => p.GetResourceCards().Count > 7))
        {
            var allDiscardedResult = gameStateManager.MoveState(ActionType.AllResourcesDiscarded);

            return allDiscardedResult;
        }

        return Result.Success();
    }

    public Result BuyDevelopmentCard()
    {
        if (remainingDevelopmentCards.Count == 0)
        {
            return Result.Failure(GameErrors.NoDevelopmentCardsLeft);
        }

        if (!CurrentPlayer.CanBuyDevelopmentCard())
        {
            return Result.Failure(PlayerErrors.MissingResources);
        }

        var card = remainingDevelopmentCards[0];

        var result = CurrentPlayer.BuyDevelopmentCard(card);

        if (result.IsFailure)
        {
            return result;
        }

        remainingDevelopmentCards.RemoveAt(0);
        remainingDevelopmentCardTotals[card]--;

        SetWinnerIndex();

        return Result.Success();
    }

    private Result HandleNextPlayerState()
    {
        if (CurrentState == GameState.SecondSettlement)
        {
            if (currentPlayerIndex == 0)
            {
                var secondSetupFinishedResult = gameStateManager.MoveState(
                    ActionType.SecondSetupFinished);

                return secondSetupFinishedResult;
            }
            else
            {
                currentPlayerIndex = (currentPlayerIndex - 1) % players.Count;
            }
        }
        else if (CurrentState == GameState.FirstSettlement)
        {
            if (currentPlayerIndex == players.Count - 1)
            {
                var firstSetupFinishedResult = gameStateManager.MoveState(
                    ActionType.FirstSetupFinished);

                return firstSetupFinishedResult;
            }
            else
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            }
        }
        else
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            var endTurnResult = gameStateManager.MoveState(ActionType.EndTurn);

            return endTurnResult;
        }

        return Result.Success();
    }

    private void SetWinnerIndex()
    {
        if (CurrentPlayer.VictoryPoints >= 10)
        {
            WinnerIndex = currentPlayerIndex;
        }
        else
        {
            WinnerIndex = null;
        }
    }

    private int? SetLargestArmyPlayerIndex()
    {
        var playerIndex = players.FindIndex(p => p.HasLargestArmy);

        if (playerIndex == -1)
        {
            return null;
        }

        return playerIndex;
    }

    private int? SetLongestRoadPlayerIndex()
    {
        var playerIndex = players.FindIndex(p => p.HasLongestRoad);

        if (playerIndex == -1)
        {
            return null;
        }

        return playerIndex;
    }

    private void InitialisePlayers(int numberOfPlayers)
    {
        players.Clear();

        for (var i = 0; i < numberOfPlayers; i++)
        {
            PlayerColour playerColour = (PlayerColour)i;
            var newPlayer = new Player(playerColour);

            players.Add(newPlayer);
        }

        ShufflePlayers();
    }

    private void ShufflePlayers()
    {
        var totalPlayers = players.Count;

        while (totalPlayers > 1)
        {
            var playerIndexToSwapWith = random.Next(totalPlayers);
            totalPlayers--;
            (players[totalPlayers], players[playerIndexToSwapWith]) = (players[playerIndexToSwapWith], players[totalPlayers]);
        }
    }

    private void InitialiseDevelopmentCards()
    {
        remainingDevelopmentCardTotals.Clear();

        var totals = DomainConstants.GetDevelopmentCardTypeTotals();

        foreach (var type in totals.Keys)
        {
            remainingDevelopmentCardTotals.Add(type, totals[type]);
            for (var i = 0; i < totals[type]; i++)
            {
                remainingDevelopmentCards.Add(type);
            }
        }

        ShuffleDevelopmentCards();
    }

    private void ShuffleDevelopmentCards()
    {
        var totalDevelopmentCards = remainingDevelopmentCards.Count;

        while (totalDevelopmentCards > 1)
        {
            var cardIndexToSwapWith = random.Next(totalDevelopmentCards);
            totalDevelopmentCards--;
            (remainingDevelopmentCards[totalDevelopmentCards], remainingDevelopmentCards[cardIndexToSwapWith]) = (remainingDevelopmentCards[cardIndexToSwapWith], remainingDevelopmentCards[totalDevelopmentCards]);
        }
    }

    private Player? GetPlayerByColour(PlayerColour colour)
    {
        return players.FirstOrDefault(p => p.Colour == colour);
    }

    private void InitialiseResourceCards()
    {
        remainingResourceCards.Clear();

        var totals = DomainConstants.GetBankResourceTotals();

        foreach (var type in totals.Keys)
        {
            remainingResourceCards.Add(type, totals[type]);
        }
    }

    private void UpdateLargestArmyPlayer()
    {
        var player = players.OrderByDescending(p => p.KnightsPlayed).FirstOrDefault();

        if (player != null && LargestArmyPlayer != player && player.KnightsPlayed >= knightsRequiredForLargestArmy)
        {
            LargestArmyPlayer?.RemoveLargestArmyCard();

            player.AddLargestArmyCard();
            knightsRequiredForLargestArmy = player.KnightsPlayed + 1;
        }
    }

    private void UpdateLargestRoadPlayer()
    {
        var longestRoadInfo = Board.GetLongestRoadInfo();

        var player = players.FirstOrDefault(p => p.Colour == longestRoadInfo.Colour);

        if (LongestRoadPlayer != player)
        {
            LongestRoadPlayer?.RemoveLongestRoadCard();

            player?.AddLongestRoadCard();
        }
    }

    private Result PlayDevelopmentCard(DevelopmentCardType type)
    {
        if (developmentCardPlayedThisTurn)
        {
            return Result.Failure(PlayerErrors.DevelopmentCardAlreadyPlayed);
        }

        var canPlayCardResult = CurrentPlayer.CanRemoveDevelopmentCard(type);

        if (canPlayCardResult.IsFailure)
        {
            return canPlayCardResult;
        }

        var handleStateResult = HandlePlayDevelopmentCardState(type);

        if (handleStateResult.IsFailure)
        {
            return handleStateResult;
        }

        var result = CurrentPlayer.RemoveDevelopmentCard(type);

        if (result.IsFailure)
        {
            throw new Exception("Failed to remove development card.");
        }

        developmentCardPlayedThisTurn = true;

        return Result.Success();
    }

    private Result HandlePlayDevelopmentCardState(DevelopmentCardType type)
    {
        if (type == DevelopmentCardType.Knight)
        {
            return gameStateManager.MoveState(ActionType.PlayKnightCard);
        }
        else if (type == DevelopmentCardType.RoadBuilding)
        {
            return gameStateManager.MoveState(ActionType.PlayRoadBuildingCard);
        }
        else if (type == DevelopmentCardType.YearOfPlenty)
        {
            return gameStateManager.MoveState(ActionType.PlayYearOfPlentyCard);
        }
        else if (type == DevelopmentCardType.Monopoly)
        {
            return gameStateManager.MoveState(ActionType.PlayMonopolyCard);
        }

        throw new InvalidOperationException($"Invalid development card type: '{type}'");
    }
}
