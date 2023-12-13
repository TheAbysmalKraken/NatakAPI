using static Catan.Common.Enumerations;

namespace Catan.Domain;

public class CatanPlayer
{
    private readonly Dictionary<CatanResourceType, int> resourceCards;
    private readonly Dictionary<CatanDevelopmentCardType, int> playableDevelopmentCards;
    private readonly Dictionary<CatanDevelopmentCardType, int> developmentCardsOnHold;

    public CatanPlayer(CatanPlayerColour colour)
    {
        Colour = colour;

        resourceCards = InitialiseResourceCards();
        playableDevelopmentCards = InitialiseDevelopmentCards();
        developmentCardsOnHold = InitialiseDevelopmentCards();
    }

    public CatanPlayerColour Colour { get; private set; }

    public int KnightsPlayed { get; set; } = 0;

    public int VictoryPoints => CalculateVictoryPoints();

    public bool HasLargestArmy { get; set; } = false;

    public bool HasLongestRoad { get; set; } = false;

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
        }

        developmentCardsOnHold.Clear();
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

        playableDevelopmentCards[type]--;
    }

    public void AddDevelopmentCard(CatanDevelopmentCardType type)
    {
        if (type == CatanDevelopmentCardType.VictoryPoint)
        {
            playableDevelopmentCards[CatanDevelopmentCardType.VictoryPoint]++;
        }
        else
        {
            developmentCardsOnHold[type]++;
        }
    }

    private Dictionary<CatanResourceType, int> InitialiseResourceCards()
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

    private Dictionary<CatanDevelopmentCardType, int> InitialiseDevelopmentCards()
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

    private int CalculateVictoryPoints()
    {
        var victoryPoints = 0;

        victoryPoints += playableDevelopmentCards[CatanDevelopmentCardType.VictoryPoint];

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
