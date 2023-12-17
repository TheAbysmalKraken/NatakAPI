using static Catan.Common.Enumerations;

namespace Catan.Domain;

public class CatanPlayer
{
    private readonly Dictionary<CatanResourceType, int> resourceCards;
    private readonly Dictionary<CatanDevelopmentCardType, int> playableDevelopmentCards;
    private readonly Dictionary<CatanDevelopmentCardType, int> developmentCardsOnHold;
    private int victoryPointDevelopmentCardCount;
    private int victoryPointsFromBuildings;

    public CatanPlayer(CatanPlayerColour colour)
    {
        Colour = colour;

        resourceCards = InitialiseResourceCards();
        playableDevelopmentCards = InitialiseDevelopmentCards();
        developmentCardsOnHold = InitialiseDevelopmentCards();

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

    public void AddLargestArmyCard() => HasLargestArmy = true;

    public void RemoveLargestArmyCard() => HasLargestArmy = false;

    public void AddLongestRoadCard() => HasLongestRoad = true;

    public void RemoveLongestRoadCard() => HasLongestRoad = false;

    public bool CanPlaceRoad()
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

    public void PlaceRoad()
    {
        resourceCards[CatanResourceType.Wood]--;
        resourceCards[CatanResourceType.Brick]--;

        RemainingRoads--;
    }

    public bool CanPlaceSettlement()
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

    public void PlaceSettlement()
    {
        resourceCards[CatanResourceType.Wood]--;
        resourceCards[CatanResourceType.Brick]--;
        resourceCards[CatanResourceType.Sheep]--;
        resourceCards[CatanResourceType.Wheat]--;

        RemainingSettlements--;
    }

    public bool CanPlaceCity()
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

    public void PlaceCity()
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

    public void PlayResourceCard(CatanResourceType type)
    {
        if (resourceCards[type] == 0)
        {
            return;
        }

        resourceCards[type]--;
    }

    public void AddResourceCard(CatanResourceType type)
    {
        resourceCards[type]++;
    }

    public void MoveOnHoldDevelopmentCardsToPlayable()
    {
        foreach (var developmentCard in developmentCardsOnHold)
        {
            playableDevelopmentCards[developmentCard.Key] += developmentCard.Value;
            developmentCardsOnHold[developmentCard.Key] = 0;
        }
    }

    public bool CanPlayDevelopmentCardOfType(CatanDevelopmentCardType type)
    {
        if (type == CatanDevelopmentCardType.VictoryPoint
            || playableDevelopmentCards[type] <= 0)
        {
            return false;
        }

        return true;
    }

    public void PlayDevelopmentCard(CatanDevelopmentCardType type)
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
