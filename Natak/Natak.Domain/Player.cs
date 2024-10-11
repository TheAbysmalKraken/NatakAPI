using Natak.Domain.Enums;
using Natak.Domain.Errors;
using Natak.Domain.Managers;

namespace Natak.Domain;

public sealed class Player
{
    public Player(
        PlayerColour colour,
        PlayerResourceCardManager resourceCardManager,
        PlayerGrowthCardManager growthCardManager,
        PlayerPieceManager pieceManager,
        PlayerScoreManager scoreManager,
        int soldiersPlayed,
        List<PortType> ports,
        int cardsToDiscard)
    {
        Colour = colour;
        ResourceCardManager = resourceCardManager;
        GrowthCardManager = growthCardManager;
        PieceManager = pieceManager;
        ScoreManager = scoreManager;
        SoldiersPlayed = soldiersPlayed;
        Ports = ports;
        CardsToDiscard = cardsToDiscard;
    }
    
    public Player(PlayerColour colour)
    {
        Colour = colour;

        SetInitialPieces();
    }

    public PlayerColour Colour { get; init; }

    public PlayerResourceCardManager ResourceCardManager { get; init; } = new();

    public PlayerGrowthCardManager GrowthCardManager { get; init; } = new();

    public PlayerPieceManager PieceManager { get; init; } = new();

    public PlayerScoreManager ScoreManager { get; init; } = new();

    public int SoldiersPlayed { get; private set; } = 0;

    public List<PortType> Ports { get; init; } = [];

    public int CardsToDiscard { get; set; } = 0;

    public void AddResourceCards(Dictionary<ResourceType, int> cards)
        => ResourceCardManager.Add(cards);

    public void AddResourceCard(ResourceType resourceType, int count = 1)
        => ResourceCardManager.Add(resourceType, count);

    public Result RemoveResourceCards(Dictionary<ResourceType, int> cards)
        => ResourceCardManager.Remove(cards);

    public Result RemoveResourceCard(ResourceType resourceType, int count = 1)
        => ResourceCardManager.Remove(resourceType, count);

    public ResourceType? RemoveRandomResourceCard()
        => ResourceCardManager.RemoveRandom();

    public Result HasResourceCards(Dictionary<ResourceType, int> cards)
    {
        var hasResources = ResourceCardManager.Has(cards);

        return hasResources
            ? Result.Success()
            : Result.Failure(PlayerErrors.MissingResources);
    }

    public Result HasResourceCard(ResourceType resourceType, int count = 1)
    {
        var hasResources = ResourceCardManager.Has(resourceType, count);

        return hasResources
            ? Result.Success()
            : Result.Failure(PlayerErrors.MissingResources);
    }

    public int CountResourceCard(ResourceType resourceType)
        => ResourceCardManager.Count(resourceType);

    public void AddGrowthCard(GrowthCardType cardType)
    {
        if (cardType == GrowthCardType.VictoryPoint)
        {
            ScoreManager.AddHiddenPoints(1);
        }

        GrowthCardManager.Add(cardType);
    }

    public Result RemoveGrowthCard(GrowthCardType cardType)
    {
        if (cardType == GrowthCardType.VictoryPoint)
        {
            ScoreManager.RemoveHiddenPoints(1);
        }

        if (cardType == GrowthCardType.Soldier)
        {
            SoldiersPlayed++;
        }

        return GrowthCardManager.Remove(cardType);
    }

    public void CycleGrowthCards() => GrowthCardManager.CycleOnHoldCards();

    public void AddPiece(BuildingType buildingType)
    {
        PieceManager.Add(buildingType);
    }

    public Result RemovePiece(BuildingType buildingType)
    {
        if (buildingType == BuildingType.Village
            || buildingType == BuildingType.Town)
        {
            ScoreManager.AddVisiblePoints(1);
        }

        return PieceManager.Remove(buildingType);
    }

    public void AddLongestRoadCard()
    {
        ScoreManager.SetHasLongestRoad(true);
    }

    public void RemoveLongestRoadCard()
    {
        ScoreManager.SetHasLongestRoad(false);
    }

    public void AddLargestArmyCard()
    {
        ScoreManager.SetHasLargestArmy(true);
    }

    public void RemoveLargestArmyCard()
    {
        ScoreManager.SetHasLargestArmy(false);
    }

    public void AddPort(PortType portType)
    {
        if (HasPort(portType))
        {
            return;
        }

        Ports.Add(portType);
    }

    public bool HasPort(PortType portType)
    {
        return Ports.Contains(portType);
    }

    private void SetInitialPieces()
    {
        PieceManager.Set(BuildingType.Road, 15);
        PieceManager.Set(BuildingType.Village, 5);
        PieceManager.Set(BuildingType.Town, 4);
    }
}
