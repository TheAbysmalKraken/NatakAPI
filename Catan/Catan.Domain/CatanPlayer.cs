using static Catan.Common.Enumerations;

namespace Catan.Domain;

public class CatanPlayer
{
    private readonly Dictionary<CatanResourceType, int> resourceCards;
    private readonly Dictionary<CatanDevelopmentCardType, int> playableDevelopmentCards;
    private readonly Dictionary<CatanDevelopmentCardType, int> developmentCardsOnHold;
    private readonly List<CatanPlayerColour> embargoedPlayers;
    private int victoryPointDevelopmentCardCount;
    private int victoryPointsFromBuildings;

    private static readonly Random random = new();

    public CatanPlayer(CatanPlayerColour colour)
    {
        Colour = colour;

        resourceCards = InitialiseResourceCards();
        playableDevelopmentCards = InitialiseDevelopmentCards();
        developmentCardsOnHold = InitialiseDevelopmentCards();
        embargoedPlayers = new List<CatanPlayerColour>();

        victoryPointsFromBuildings = 0;
        victoryPointDevelopmentCardCount = 0;
    }

    public CatanPlayerColour Colour { get; private set; }

    public int KnightsPlayed { get; private set; } = 0;

    public int VictoryPoints
        => victoryPointsFromBuildings + CalculateVictoryPointsExcludingBuildings();

    public bool HasLargestArmy { get; private set; } = false;

    public bool HasLongestRoad { get; private set; } = false;

    public int RemainingSettlements { get; private set; } = 0;

    public int RemainingCities { get; private set; } = 0;

    public int RemainingRoads { get; private set; } = 0;

    public Dictionary<CatanResourceType, int> GetResourceCards()
    {
        return resourceCards;
    }

    public Dictionary<CatanDevelopmentCardType, int> GetPlayableDevelopmentCards()
    {
        return playableDevelopmentCards;
    }

    public Dictionary<CatanDevelopmentCardType, int> GetDevelopmentCardsOnHold()
    {
        return developmentCardsOnHold;
    }

    public List<CatanPlayerColour> GetEmbargoedPlayers()
    {
        return embargoedPlayers;
    }

    public void AddLargestArmyCard() => HasLargestArmy = true;

    public void RemoveLargestArmyCard() => HasLargestArmy = false;

    public void AddLongestRoadCard() => HasLongestRoad = true;

    public void RemoveLongestRoadCard() => HasLongestRoad = false;

    public void EmbargoPlayer(CatanPlayerColour colour)
    {
        if (colour == CatanPlayerColour.None || colour == Colour)
        {
            throw new ArgumentException("Player cannot embargo themselves or no player.");
        }

        if (embargoedPlayers.Contains(colour))
        {
            return;
        }

        embargoedPlayers.Add(colour);
    }

    public void RemoveEmbargo(CatanPlayerColour colour)
    {
        if (colour == CatanPlayerColour.None || colour == Colour)
        {
            throw new ArgumentException("Player cannot remove embargo from themselves or no player.");
        }

        embargoedPlayers.Remove(colour);
    }

    public bool CanBuyRoad()
    {
        if (RemainingRoads <= 0)
        {
            return false;
        }

        if (resourceCards[CatanResourceType.Wood] < 1
        || resourceCards[CatanResourceType.Brick] < 1)
        {
            return false;
        }

        return true;
    }

    public void BuyRoad()
    {
        resourceCards[CatanResourceType.Wood]--;
        resourceCards[CatanResourceType.Brick]--;

        RemainingRoads--;
    }

    public bool CanBuySettlement()
    {
        if (RemainingSettlements <= 0)
        {
            return false;
        }

        if (resourceCards[CatanResourceType.Wood] < 1
        || resourceCards[CatanResourceType.Brick] < 1
        || resourceCards[CatanResourceType.Sheep] < 1
        || resourceCards[CatanResourceType.Wheat] < 1)
        {
            return false;
        }

        return true;
    }

    public void BuySettlement()
    {
        resourceCards[CatanResourceType.Wood]--;
        resourceCards[CatanResourceType.Brick]--;
        resourceCards[CatanResourceType.Sheep]--;
        resourceCards[CatanResourceType.Wheat]--;

        RemainingSettlements--;
    }

    public bool CanBuyCity()
    {
        if (RemainingCities <= 0)
        {
            return false;
        }

        if (resourceCards[CatanResourceType.Wheat] < 2
        || resourceCards[CatanResourceType.Ore] < 3)
        {
            return false;
        }

        return true;
    }

    public void BuyCity()
    {
        resourceCards[CatanResourceType.Wheat] -= 2;
        resourceCards[CatanResourceType.Ore] -= 3;

        RemainingCities--;
    }

    public bool CanBuyDevelopmentCard()
    {
        if (resourceCards[CatanResourceType.Sheep] < 1
        || resourceCards[CatanResourceType.Wheat] < 1
        || resourceCards[CatanResourceType.Ore] < 1)
        {
            return false;
        }

        return true;
    }

    public void BuyDevelopmentCard(CatanDevelopmentCardType type)
    {
        if (type == CatanDevelopmentCardType.VictoryPoint)
        {
            victoryPointDevelopmentCardCount++;
        }
        else
        {
            resourceCards[CatanResourceType.Sheep]--;
            resourceCards[CatanResourceType.Wheat]--;
            resourceCards[CatanResourceType.Ore]--;

            developmentCardsOnHold[type]++;
        }
    }

    public bool CanTradeTwoToOneOfCardType(CatanResourceType type)
    {
        if (resourceCards[type] < 2)
        {
            return false;
        }

        return true;
    }

    public void TradeTwoToOne(
        CatanResourceType typeToGive,
        CatanResourceType typeToReceive)
    {
        resourceCards[typeToGive] -= 2;
        resourceCards[typeToReceive]++;
    }

    public bool CanTradeThreeToOneOfCardType(CatanResourceType type)
    {
        if (resourceCards[type] < 3)
        {
            return false;
        }

        return true;
    }

    public void TradeThreeToOne(
        CatanResourceType typeToGive,
        CatanResourceType typeToReceive)
    {
        resourceCards[typeToGive] -= 3;
        resourceCards[typeToReceive]++;
    }

    public bool CanTradeFourToOneOfCardType(CatanResourceType type)
    {
        if (resourceCards[type] < 4)
        {
            return false;
        }

        return true;
    }

    public void TradeFourToOne(
        CatanResourceType typeToGive,
        CatanResourceType typeToReceive)
    {
        resourceCards[typeToGive] -= 4;
        resourceCards[typeToReceive]++;
    }

    public bool CanDiscardResourceCards(Dictionary<CatanResourceType, int> cardsToDiscard)
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

    public void DiscardResourceCards(Dictionary<CatanResourceType, int> cardsToDiscard)
    {
        if (!CanDiscardResourceCards(cardsToDiscard))
        {
            throw new ArgumentException("Player does not have enough of the specified resource cards to discard.");
        }

        foreach (var type in cardsToDiscard.Keys)
        {
            resourceCards[type] -= cardsToDiscard[type];
        }
    }

    public bool CanRemoveResourceCard(CatanResourceType type, int count = 1)
    {
        return resourceCards[type] >= count;
    }

    public void RemoveResourceCard(CatanResourceType type, int count = 1)
    {
        if (!CanRemoveResourceCard(type, count))
        {
            throw new ArgumentException("Player does not have the specified resource card to play.");
        }

        resourceCards[type] -= count;
    }

    public CatanResourceType? RemoveRandomResourceCard()
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

    public void AddResourceCard(CatanResourceType type, int count = 1)
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

    public bool CanRemoveDevelopmentCard(CatanDevelopmentCardType type)
    {
        if (type == CatanDevelopmentCardType.VictoryPoint
            || playableDevelopmentCards[type] <= 0)
        {
            return false;
        }

        return true;
    }

    public void RemoveDevelopmentCard(CatanDevelopmentCardType type)
    {
        if (type == CatanDevelopmentCardType.VictoryPoint)
        {
            return;
        }

        if (playableDevelopmentCards[type] == 0)
        {
            return;
        }

        if (type == CatanDevelopmentCardType.Knight)
        {
            KnightsPlayed++;
        }

        playableDevelopmentCards[type]--;
    }

    public void SetVictoryPointsFromBuildings(int settlementCount, int cityCount)
    {
        if (settlementCount < 0 || cityCount < 0)
        {
            throw new ArgumentException("Method arguments must be positive integers.");
        }

        victoryPointsFromBuildings = settlementCount + 2 * cityCount;
    }

    private static Dictionary<CatanResourceType, int> InitialiseResourceCards()
    {
        return new Dictionary<CatanResourceType, int>()
        {
            { CatanResourceType.Wood, 0 },
            { CatanResourceType.Brick, 0 },
            { CatanResourceType.Sheep, 0 },
            { CatanResourceType.Wheat, 0 },
            { CatanResourceType.Ore, 0 }
        };
    }

    private static Dictionary<CatanDevelopmentCardType, int> InitialiseDevelopmentCards()
    {
        return new Dictionary<CatanDevelopmentCardType, int>()
        {
            { CatanDevelopmentCardType.Knight, 0 },
            { CatanDevelopmentCardType.RoadBuilding, 0 },
            { CatanDevelopmentCardType.YearOfPlenty, 0 },
            { CatanDevelopmentCardType.Monopoly, 0 },
            { CatanDevelopmentCardType.VictoryPoint, 0 }
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
