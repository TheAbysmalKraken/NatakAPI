using Catan.Domain.Enums;

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
    private readonly Random random = new();

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

        GamePhase = GamePhase.FirstRoundSetup;
        GameSubPhase = GameSubPhase.BuildSettlement;
        PlayerCount = numberOfPlayers;
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

    public GamePhase GamePhase { get; private set; }

    public GameSubPhase GameSubPhase { get; private set; }

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

    public void NextPlayer()
    {
        UpdateLargestArmyPlayer();

        developmentCardPlayedThisTurn = false;

        CurrentPlayer.MoveOnHoldDevelopmentCardsToPlayable();

        if (GamePhase == GamePhase.SecondRoundSetup)
        {
            if (currentPlayerIndex == 0)
            {
                GamePhase = GamePhase.Main;
                GameSubPhase = GameSubPhase.RollOrPlayDevelopmentCard;
            }
            else
            {
                currentPlayerIndex = (currentPlayerIndex - 1) % players.Count;
            }
        }
        else if (GamePhase == GamePhase.FirstRoundSetup)
        {
            if (currentPlayerIndex == players.Count - 1)
            {
                GamePhase = GamePhase.SecondRoundSetup;
            }
            else
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            }
        }
        else
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            GamePhase = GamePhase.Main;
            GameSubPhase = GameSubPhase.RollOrPlayDevelopmentCard;
        }
    }

    public void RollDice()
    {
        rolledDice.Clear();
        rolledDice.AddRange(DiceRoller.RollDice(2, 6));
    }

    public bool RollDiceAndDistributeResourcesToPlayers()
    {
        RollDice();

        var diceTotal = DiceTotal;

        if (diceTotal == 7)
        {
            GameSubPhase = GameSubPhase.DiscardResources;

            TryFinishDiscardingResources();

            return false;
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

        GameSubPhase = GameSubPhase.TradeOrBuild;

        return true;
    }

    public bool GiveResourcesSurroundingHouse(Point point)
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

        return true;
    }

    public bool TradeTwoToOne(ResourceType resourceTypeToGive, ResourceType resourceTypeToReceive)
    {
        var portTypeValid = Enum.TryParse<PortType>(resourceTypeToReceive.ToString(), out var portType);

        if (!portTypeValid)
        {
            return false;
        }

        if (!CurrentPlayer.CanTradeTwoToOneOfCardType(resourceTypeToGive)
        || !Board.ColourHasPortOfType(CurrentPlayer.Colour, portType))
        {
            return false;
        }

        CurrentPlayer.TradeTwoToOne(resourceTypeToGive, resourceTypeToReceive);

        return true;
    }

    public bool TradeThreeToOne(ResourceType resourceTypeToGive, ResourceType resourceTypeToReceive)
    {
        if (!CurrentPlayer.CanTradeThreeToOneOfCardType(resourceTypeToGive)
        || !Board.ColourHasPortOfType(CurrentPlayer.Colour, PortType.ThreeToOne))
        {
            return false;
        }

        CurrentPlayer.TradeThreeToOne(resourceTypeToGive, resourceTypeToReceive);

        return true;
    }

    public bool TradeFourToOne(ResourceType resourceTypeToGive, ResourceType resourceTypeToReceive)
    {
        if (!CurrentPlayer.CanTradeFourToOneOfCardType(resourceTypeToGive))
        {
            return false;
        }

        CurrentPlayer.TradeFourToOne(resourceTypeToGive, resourceTypeToReceive);

        return true;
    }

    public bool EmbargoPlayer(PlayerColour colourEmbargoedBy, PlayerColour colourToEmbargo)
    {
        if (colourEmbargoedBy == colourToEmbargo
        || colourEmbargoedBy == PlayerColour.None
        || colourToEmbargo == PlayerColour.None)
        {
            return false;
        }

        var playerEmbargoedBy = GetPlayerByColour(colourEmbargoedBy);

        if (playerEmbargoedBy == null)
        {
            return false;
        }

        playerEmbargoedBy.EmbargoPlayer(colourToEmbargo);

        return true;
    }

    public bool RemovePlayerEmbargo(PlayerColour colourEmbargoedBy, PlayerColour colourToEmbargo)
    {
        if (colourEmbargoedBy == colourToEmbargo
        || colourEmbargoedBy == PlayerColour.None
        || colourToEmbargo == PlayerColour.None)
        {
            return false;
        }

        var playerEmbargoedBy = GetPlayerByColour(colourEmbargoedBy);

        if (playerEmbargoedBy == null)
        {
            return false;
        }

        playerEmbargoedBy.RemoveEmbargo(colourToEmbargo);

        return true;
    }

    public bool PlayKnightCard(Point robberPoint, PlayerColour colourToStealFrom)
    {
        if (!CanPlayDevelopmentCard(DevelopmentCardType.Knight)
        || !Board.GetHouseColoursOnTile(robberPoint).Contains(colourToStealFrom))
        {
            return false;
        }

        var originalRobberPoint = Board.RobberPosition;

        if (GameSubPhase == GameSubPhase.RollOrPlayDevelopmentCard)
        {
            GameSubPhase = GameSubPhase.MoveRobberKnightCardBeforeRoll;
        }
        else if (GameSubPhase == GameSubPhase.TradeOrBuild
        || GameSubPhase == GameSubPhase.PlayTurn)
        {
            GameSubPhase = GameSubPhase.MoveRobberKnightCardAfterRoll;
        }

        var moveRobberSuccess = MoveRobber(robberPoint);

        if (!moveRobberSuccess)
        {
            return false;
        }

        var stealResourceSuccess = StealResourceCard(colourToStealFrom);

        if (!stealResourceSuccess)
        {
            Board.MoveRobberToPoint(originalRobberPoint);
            return false;
        }

        PlayDevelopmentCard(DevelopmentCardType.Knight);

        UpdateLargestArmyPlayer();

        SetWinnerIndex();

        return true;
    }

    public bool PlayYearOfPlentyCard(ResourceType resourceType1, ResourceType resourceType2)
    {
        if (resourceType1 == ResourceType.None || resourceType2 == ResourceType.None)
        {
            return false;
        }

        if (!CanPlayDevelopmentCard(DevelopmentCardType.YearOfPlenty))
        {
            return false;
        }

        var resourceTotal1 = remainingResourceCards[resourceType1];
        var resourceTotal2 = remainingResourceCards[resourceType2];

        if (resourceTotal1 == 0 || resourceTotal2 == 0)
        {
            return false;
        }

        if (resourceType1 == resourceType2 && resourceTotal1 < 2)
        {
            return false;
        }

        CurrentPlayer.AddResourceCard(resourceType1);
        CurrentPlayer.AddResourceCard(resourceType2);

        PlayDevelopmentCard(DevelopmentCardType.YearOfPlenty);

        return true;
    }

    public bool PlayMonopolyCard(ResourceType resourceType)
    {
        if (resourceType == ResourceType.None)
        {
            return false;
        }

        if (!CanPlayDevelopmentCard(DevelopmentCardType.Monopoly))
        {
            return false;
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

        PlayDevelopmentCard(DevelopmentCardType.Monopoly);

        return true;
    }

    public bool PlayRoadBuildingCard(
        Point point1,
        Point point2,
        Point point3,
        Point point4)
    {
        if (!CanPlayDevelopmentCard(DevelopmentCardType.RoadBuilding)
        || !Board.CanPlaceTwoRoadsBetweenPoints(point1, point2, point3, point4, CurrentPlayer.Colour))
        {
            return false;
        }

        Board.PlaceRoad(point1, point2, CurrentPlayer.Colour);
        Board.PlaceRoad(point3, point4, CurrentPlayer.Colour);

        PlayDevelopmentCard(DevelopmentCardType.RoadBuilding);

        UpdateLargestRoadPlayer();

        SetWinnerIndex();

        return true;
    }

    public bool MakeTradeWithPlayer(
        PlayerColour otherPlayerColour,
        Dictionary<ResourceType, int> resourcesGivenByCurrentPlayer,
        Dictionary<ResourceType, int> resourcesReceivedByCurrentPlayer)
    {
        var playerToTradeWith = GetPlayerByColour(otherPlayerColour);

        if (playerToTradeWith == null
        || !CurrentPlayer.CanTradeWithPlayer(otherPlayerColour)
        || !playerToTradeWith.CanTradeWithPlayer(CurrentPlayer.Colour)
        || !CurrentPlayer.HasAdequateResourceCardsOfTypes(resourcesGivenByCurrentPlayer)
        || !playerToTradeWith.HasAdequateResourceCardsOfTypes(resourcesReceivedByCurrentPlayer))
        {
            return false;
        }

        CurrentPlayer.AddResourceCards(resourcesReceivedByCurrentPlayer);
        CurrentPlayer.RemoveResourceCards(resourcesGivenByCurrentPlayer);
        playerToTradeWith.AddResourceCards(resourcesGivenByCurrentPlayer);
        playerToTradeWith.RemoveResourceCards(resourcesReceivedByCurrentPlayer);

        return true;
    }

    public bool BuildFreeRoad(Point point1, Point point2)
    {
        if (!Board.CanPlaceRoadBetweenPoints(point1, point2, CurrentPlayer.Colour))
        {
            return false;
        }

        CurrentPlayer.BuyFreeRoad();
        Board.PlaceRoad(point1, point2, CurrentPlayer.Colour);

        if (GamePhase == GamePhase.FirstRoundSetup
        || GamePhase == GamePhase.SecondRoundSetup)
        {
            GameSubPhase = GameSubPhase.BuildSettlement;
            NextPlayer();
        }

        return true;
    }

    public bool BuildRoad(Point point1, Point point2)
    {
        if (!CurrentPlayer.CanBuyRoad() || !Board.CanPlaceRoadBetweenPoints(point1, point2, CurrentPlayer.Colour))
        {
            return false;
        }

        CurrentPlayer.BuyRoad();
        Board.PlaceRoad(point1, point2, CurrentPlayer.Colour);

        UpdateLargestRoadPlayer();

        SetWinnerIndex();

        return true;
    }

    public bool BuildFreeSettlement(Point point)
    {
        if (!Board.CanPlaceHouseAtPoint(point, CurrentPlayer.Colour, true))
        {
            return false;
        }

        CurrentPlayer.BuyFreeSettlement();
        Board.PlaceHouse(point, CurrentPlayer.Colour, true);

        if (GamePhase == GamePhase.SecondRoundSetup)
        {
            GiveResourcesSurroundingHouse(point);
        }

        if (GamePhase == GamePhase.FirstRoundSetup
        || GamePhase == GamePhase.SecondRoundSetup)
        {
            GameSubPhase = GameSubPhase.BuildRoad;
        }

        return true;
    }

    public bool BuildSettlement(Point point)
    {
        if (!CurrentPlayer.CanBuySettlement() || !Board.CanPlaceHouseAtPoint(point, CurrentPlayer.Colour))
        {
            return false;
        }

        CurrentPlayer.BuySettlement();
        Board.PlaceHouse(point, CurrentPlayer.Colour);

        UpdateLargestRoadPlayer();

        SetWinnerIndex();

        return true;
    }

    public bool BuildCity(Point point)
    {
        if (!CurrentPlayer.CanBuyCity() || !Board.CanUpgradeHouseAtPoint(point, CurrentPlayer.Colour))
        {
            return false;
        }

        CurrentPlayer.BuyCity();
        Board.UpgradeHouse(point, CurrentPlayer.Colour);

        SetWinnerIndex();

        return true;
    }

    public bool MoveRobber(Point point)
    {
        if (!Board.CanMoveRobberToPoint(point))
        {
            return false;
        }

        Board.MoveRobberToPoint(point);

        if (GameSubPhase == GameSubPhase.MoveRobberKnightCardBeforeRoll)
        {
            GameSubPhase = GameSubPhase.StealResourceKnightCardBeforeRoll;
        }
        else if (GameSubPhase == GameSubPhase.MoveRobberSevenRoll)
        {
            GameSubPhase = GameSubPhase.StealResourceKnightCardAfterRoll;
        }

        return true;
    }

    public bool StealResourceCard(PlayerColour victimColour)
    {
        var victim = GetPlayerByColour(victimColour);

        if (victim == null || victim == CurrentPlayer)
        {
            return false;
        }

        var stolenCard = victim.RemoveRandomResourceCard();

        if (stolenCard != null)
        {
            CurrentPlayer.AddResourceCard(stolenCard.Value);
        }

        if (GameSubPhase == GameSubPhase.StealResourceKnightCardBeforeRoll)
        {
            GameSubPhase = GameSubPhase.Roll;
        }
        else if (GameSubPhase == GameSubPhase.StealResourceKnightCardAfterRoll)
        {
            GameSubPhase = GameSubPhase.TradeOrBuild;
        }

        return true;
    }

    public bool DiscardResources(PlayerColour playerColourDiscarding, Dictionary<ResourceType, int> resourcesToDiscard)
    {
        var player = GetPlayerByColour(playerColourDiscarding);

        if (player == null)
        {
            return false;
        }

        if (!player.HasAdequateResourceCardsOfTypes(resourcesToDiscard))
        {
            return false;
        }

        player.RemoveResourceCards(resourcesToDiscard);

        foreach (var type in resourcesToDiscard.Keys)
        {
            remainingResourceCards[type] += resourcesToDiscard[type];
        }

        return true;
    }

    public void TryFinishDiscardingResources()
    {
        if (!players.Any(p => p.GetResourceCards().Count > 7))
        {
            GameSubPhase = GameSubPhase.MoveRobberSevenRoll;
        }
    }

    public bool BuyDevelopmentCard()
    {
        if (!CurrentPlayer.CanBuyDevelopmentCard() || remainingDevelopmentCards.Count == 0)
        {
            return false;
        }

        var card = remainingDevelopmentCards[0];
        remainingDevelopmentCards.RemoveAt(0);
        remainingDevelopmentCardTotals[card]--;

        CurrentPlayer.BuyDevelopmentCard(card);

        SetWinnerIndex();

        return true;
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

    private bool CanPlayDevelopmentCard(DevelopmentCardType type)
    {
        return !developmentCardPlayedThisTurn && CurrentPlayer.CanRemoveDevelopmentCard(type);
    }

    private void PlayDevelopmentCard(DevelopmentCardType type)
    {
        if (!CanPlayDevelopmentCard(type))
        {
            throw new InvalidOperationException($"Cannot play development card of type: '{type}'");
        }

        developmentCardPlayedThisTurn = true;

        CurrentPlayer.RemoveDevelopmentCard(type);
    }
}
