using static Catan.Common.Enumerations;

namespace Catan.Domain;

public class Player(PlayerColour colour)
{
    private readonly Dictionary<ResourceType, int> resourceCards = InitialiseResourceCards();
    private readonly Dictionary<DevelopmentCardType, int> playableDevelopmentCards = InitialiseDevelopmentCards();
    private readonly Dictionary<DevelopmentCardType, int> developmentCardsOnHold = InitialiseDevelopmentCards();
    private readonly List<PlayerColour> embargoedPlayers = [];
    private int victoryPointDevelopmentCardCount = 0;
    private int victoryPointsFromBuildings = 0;

    private readonly Random random = new();

    public PlayerColour Colour { get; private set; } = colour;

    public int KnightsPlayed { get; private set; } = 0;

    public int VictoryPoints
        => victoryPointsFromBuildings + CalculateVictoryPointsExcludingBuildings();

    public int VictoryPointCards
        => victoryPointDevelopmentCardCount;

    public bool HasLargestArmy { get; private set; } = false;

    public bool HasLongestRoad { get; private set; } = false;

    public int RemainingSettlements { get; private set; } = 5;

    public int RemainingCities { get; private set; } = 4;

    public int RemainingRoads { get; private set; } = 15;

    public Dictionary<ResourceType, int> GetResourceCards()
    {
        return resourceCards;
    }

    public Dictionary<DevelopmentCardType, int> GetPlayableDevelopmentCards()
    {
        return playableDevelopmentCards;
    }

    public Dictionary<DevelopmentCardType, int> GetDevelopmentCardsOnHold()
    {
        return developmentCardsOnHold;
    }

    public List<PlayerColour> GetEmbargoedPlayers()
    {
        return embargoedPlayers;
    }

    public void AddLargestArmyCard() => HasLargestArmy = true;

    public void RemoveLargestArmyCard() => HasLargestArmy = false;

    public void AddLongestRoadCard() => HasLongestRoad = true;

    public void RemoveLongestRoadCard() => HasLongestRoad = false;

    public void EmbargoPlayer(PlayerColour colour)
    {
        if (colour == PlayerColour.None || colour == Colour)
        {
            throw new ArgumentException("Player cannot embargo themselves or no player.");
        }

        if (embargoedPlayers.Contains(colour))
        {
            return;
        }

        embargoedPlayers.Add(colour);
    }

    public void RemoveEmbargo(PlayerColour colour)
    {
        if (colour == PlayerColour.None || colour == Colour)
        {
            throw new ArgumentException("Player cannot remove embargo from themselves or no player.");
        }

        embargoedPlayers.Remove(colour);
    }

    public bool CanTradeWithPlayer(PlayerColour colour)
    {
        return !embargoedPlayers.Contains(colour) && colour != Colour;
    }

    public bool CanBuyRoad()
    {
        if (RemainingRoads <= 0)
        {
            return false;
        }

        if (resourceCards[ResourceType.Wood] < 1
        || resourceCards[ResourceType.Brick] < 1)
        {
            return false;
        }

        return true;
    }

    public void BuyRoad()
    {
        resourceCards[ResourceType.Wood]--;
        resourceCards[ResourceType.Brick]--;

        RemainingRoads--;
    }

    public void BuyFreeRoad()
    {
        RemainingRoads--;
    }

    public bool CanBuySettlement()
    {
        if (RemainingSettlements <= 0)
        {
            return false;
        }

        if (resourceCards[ResourceType.Wood] < 1
        || resourceCards[ResourceType.Brick] < 1
        || resourceCards[ResourceType.Sheep] < 1
        || resourceCards[ResourceType.Wheat] < 1)
        {
            return false;
        }

        return true;
    }

    public void BuySettlement()
    {
        resourceCards[ResourceType.Wood]--;
        resourceCards[ResourceType.Brick]--;
        resourceCards[ResourceType.Sheep]--;
        resourceCards[ResourceType.Wheat]--;

        RemainingSettlements--;

        victoryPointsFromBuildings++;
    }

    public void BuyFreeSettlement()
    {
        RemainingSettlements--;

        victoryPointsFromBuildings++;
    }

    public bool CanBuyCity()
    {
        if (RemainingCities <= 0)
        {
            return false;
        }

        if (resourceCards[ResourceType.Wheat] < 2
        || resourceCards[ResourceType.Ore] < 3)
        {
            return false;
        }

        return true;
    }

    public void BuyCity()
    {
        resourceCards[ResourceType.Wheat] -= 2;
        resourceCards[ResourceType.Ore] -= 3;

        RemainingCities--;

        victoryPointsFromBuildings++;
    }

    public bool CanBuyDevelopmentCard()
    {
        if (resourceCards[ResourceType.Sheep] < 1
        || resourceCards[ResourceType.Wheat] < 1
        || resourceCards[ResourceType.Ore] < 1)
        {
            return false;
        }

        return true;
    }

    public void BuyDevelopmentCard(DevelopmentCardType type)
    {
        if (type == DevelopmentCardType.VictoryPoint)
        {
            victoryPointDevelopmentCardCount++;
        }
        else
        {
            resourceCards[ResourceType.Sheep]--;
            resourceCards[ResourceType.Wheat]--;
            resourceCards[ResourceType.Ore]--;

            developmentCardsOnHold[type]++;
        }
    }

    public bool CanTradeTwoToOneOfCardType(ResourceType type)
    {
        if (resourceCards[type] < 2)
        {
            return false;
        }

        return true;
    }

    public void TradeTwoToOne(
        ResourceType typeToGive,
        ResourceType typeToReceive)
    {
        resourceCards[typeToGive] -= 2;
        resourceCards[typeToReceive]++;
    }

    public bool CanTradeThreeToOneOfCardType(ResourceType type)
    {
        if (resourceCards[type] < 3)
        {
            return false;
        }

        return true;
    }

    public void TradeThreeToOne(
        ResourceType typeToGive,
        ResourceType typeToReceive)
    {
        resourceCards[typeToGive] -= 3;
        resourceCards[typeToReceive]++;
    }

    public bool CanTradeFourToOneOfCardType(ResourceType type)
    {
        if (resourceCards[type] < 4)
        {
            return false;
        }

        return true;
    }

    public void TradeFourToOne(
        ResourceType typeToGive,
        ResourceType typeToReceive)
    {
        resourceCards[typeToGive] -= 4;
        resourceCards[typeToReceive]++;
    }

    public bool HasAdequateResourceCardsOfTypes(Dictionary<ResourceType, int> cardsToDiscard)
    {
        foreach (var type in cardsToDiscard.Keys)
        {
            if (resourceCards[type] < cardsToDiscard[type])
            {
                return false;
            }
        }

        return true;
    }

    public void AddResourceCards(Dictionary<ResourceType, int> cardsToAdd)
    {
        foreach (var type in cardsToAdd.Keys)
        {
            resourceCards[type] += cardsToAdd[type];
        }
    }

    public void RemoveResourceCards(Dictionary<ResourceType, int> cardsToDiscard)
    {
        if (!HasAdequateResourceCardsOfTypes(cardsToDiscard))
        {
            throw new ArgumentException("Player does not have enough of the specified resource cards to discard.");
        }

        foreach (var type in cardsToDiscard.Keys)
        {
            resourceCards[type] -= cardsToDiscard[type];
        }
    }

    public bool CanRemoveResourceCard(ResourceType type, int count = 1)
    {
        return resourceCards[type] >= count;
    }

    public void RemoveResourceCard(ResourceType type, int count = 1)
    {
        if (!CanRemoveResourceCard(type, count))
        {
            throw new ArgumentException("Player does not have the specified resource card to play.");
        }

        resourceCards[type] -= count;
    }

    public ResourceType? RemoveRandomResourceCard()
    {
        var availableResourceTypes = resourceCards.Keys.Where(k => resourceCards[k] > 0).ToList();

        if (availableResourceTypes.Count == 0)
        {
            return null;
        }

        var randomIndex = random.Next(0, availableResourceTypes.Count);

        var resourceType = availableResourceTypes[randomIndex];
        resourceCards[resourceType]--;

        return resourceType;
    }

    public void AddResourceCard(ResourceType type, int count = 1)
    {
        resourceCards[type] += count;
    }

    public void MoveOnHoldDevelopmentCardsToPlayable()
    {
        foreach (var developmentCard in developmentCardsOnHold)
        {
            playableDevelopmentCards[developmentCard.Key] += developmentCard.Value;
            developmentCardsOnHold[developmentCard.Key] = 0;
        }
    }

    public bool CanRemoveDevelopmentCard(DevelopmentCardType type)
    {
        if (type == DevelopmentCardType.VictoryPoint
            || playableDevelopmentCards[type] <= 0)
        {
            return false;
        }

        return true;
    }

    public void RemoveDevelopmentCard(DevelopmentCardType type)
    {
        if (type == DevelopmentCardType.VictoryPoint)
        {
            return;
        }

        if (playableDevelopmentCards[type] == 0)
        {
            return;
        }

        if (type == DevelopmentCardType.Knight)
        {
            KnightsPlayed++;
        }

        playableDevelopmentCards[type]--;
    }

    private static Dictionary<ResourceType, int> InitialiseResourceCards()
    {
        return new Dictionary<ResourceType, int>()
        {
            { ResourceType.Wood, 0 },
            { ResourceType.Brick, 0 },
            { ResourceType.Sheep, 0 },
            { ResourceType.Wheat, 0 },
            { ResourceType.Ore, 0 }
        };
    }

    private static Dictionary<DevelopmentCardType, int> InitialiseDevelopmentCards()
    {
        return new Dictionary<DevelopmentCardType, int>()
        {
            { DevelopmentCardType.Knight, 0 },
            { DevelopmentCardType.RoadBuilding, 0 },
            { DevelopmentCardType.YearOfPlenty, 0 },
            { DevelopmentCardType.Monopoly, 0 },
            { DevelopmentCardType.VictoryPoint, 0 }
        };
    }

    private int CalculateVictoryPointsExcludingBuildings()
    {
        var victoryPoints = 0;

        victoryPoints += victoryPointDevelopmentCardCount;

        if (HasLargestArmy)
        {
            victoryPoints += 2;
        }

        if (HasLongestRoad)
        {
            victoryPoints += 2;
        }

        return victoryPoints;
    }
}
