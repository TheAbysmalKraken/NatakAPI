using Catan.Domain.Enums;
using Catan.Domain.Errors;

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

    public Result EmbargoPlayer(PlayerColour colour)
    {
        if (colour == Colour)
        {
            return Result.Failure(PlayerErrors.CannotEmbargoSelf);
        }

        if (colour == PlayerColour.None)
        {
            return Result.Failure(PlayerErrors.InvalidPlayerColour);
        }

        if (embargoedPlayers.Contains(colour))
        {
            return Result.Failure(PlayerErrors.AlreadyEmbargoed);
        }

        embargoedPlayers.Add(colour);

        return Result.Success();
    }

    public Result RemoveEmbargo(PlayerColour colour)
    {
        if (colour == Colour)
        {
            return Result.Failure(PlayerErrors.CannotEmbargoSelf);
        }

        if (colour == PlayerColour.None)
        {
            return Result.Failure(PlayerErrors.InvalidPlayerColour);
        }

        if (!embargoedPlayers.Contains(colour))
        {
            return Result.Failure(PlayerErrors.NotEmbargoed);
        }

        embargoedPlayers.Remove(colour);

        return Result.Success();
    }

    public bool CanTradeWithPlayer(PlayerColour colour)
    {
        return !embargoedPlayers.Contains(colour) && colour != Colour;
    }

    public Result CanMakeTradeOffer(Dictionary<ResourceType, int> offer)
    {
        if (!HasAdequateResourceCardsOfTypes(offer))
        {
            return Result.Failure(PlayerErrors.MissingResources);
        }

        return Result.Success();
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
        RemainingSettlements++;

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

    public Result BuyDevelopmentCard(DevelopmentCardType type)
    {
        if (!CanBuyDevelopmentCard())
        {
            return Result.Failure(PlayerErrors.MissingResources);
        }

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

        return Result.Success();
    }

    public bool CanTradeTwoToOneOfCardType(ResourceType type)
    {
        if (resourceCards[type] < 2)
        {
            return false;
        }

        return true;
    }

    public Result TradeTwoToOne(
        ResourceType typeToGive,
        ResourceType typeToReceive)
    {
        if (!CanTradeTwoToOneOfCardType(typeToGive))
        {
            return Result.Failure(PlayerErrors.MissingResources);
        }

        resourceCards[typeToGive] -= 2;
        resourceCards[typeToReceive]++;

        return Result.Success();
    }

    public bool CanTradeThreeToOneOfCardType(ResourceType type)
    {
        if (resourceCards[type] < 3)
        {
            return false;
        }

        return true;
    }

    public Result TradeThreeToOne(
        ResourceType typeToGive,
        ResourceType typeToReceive)
    {
        if (!CanTradeThreeToOneOfCardType(typeToGive))
        {
            return Result.Failure(PlayerErrors.MissingResources);
        }

        resourceCards[typeToGive] -= 3;
        resourceCards[typeToReceive]++;

        return Result.Success();
    }

    public bool CanTradeFourToOneOfCardType(ResourceType type)
    {
        if (resourceCards[type] < 4)
        {
            return false;
        }

        return true;
    }

    public Result TradeFourToOne(
        ResourceType typeToGive,
        ResourceType typeToReceive)
    {
        if (!CanTradeFourToOneOfCardType(typeToGive))
        {
            return Result.Failure(PlayerErrors.MissingResources);
        }

        resourceCards[typeToGive] -= 4;
        resourceCards[typeToReceive]++;

        return Result.Success();
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

    public Result RemoveResourceCards(Dictionary<ResourceType, int> cardsToDiscard)
    {
        if (!HasAdequateResourceCardsOfTypes(cardsToDiscard))
        {
            return Result.Failure(PlayerErrors.MissingResources);
        }

        foreach (var type in cardsToDiscard.Keys)
        {
            resourceCards[type] -= cardsToDiscard[type];
        }

        return Result.Success();
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

    public Result CanRemoveDevelopmentCard(DevelopmentCardType type)
    {
        if (type == DevelopmentCardType.VictoryPoint)
        {
            return Result.Failure(PlayerErrors.CannotPlayVictoryPointCard);
        }

        if (playableDevelopmentCards[type] <= 0)
        {
            return Result.Failure(PlayerErrors.NoDevelopmentCardsOfType);
        }

        return Result.Success();
    }

    public Result RemoveDevelopmentCard(DevelopmentCardType type)
    {
        var canRemoveResult = CanRemoveDevelopmentCard(type);

        if (canRemoveResult.IsFailure)
        {
            return canRemoveResult;
        }

        if (type == DevelopmentCardType.Knight)
        {
            KnightsPlayed++;
        }

        playableDevelopmentCards[type]--;

        return Result.Success();
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
