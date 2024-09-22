using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Domain.Managers;

public sealed class BankTradeManager
{
    private readonly ItemManager<ResourceType> resourceCards = new();

    private readonly ItemManager<DevelopmentCardType> developmentCards = new();

    public Dictionary<ResourceType, int> ResourceCards => resourceCards.Items;

    public Dictionary<DevelopmentCardType, int> DevelopmentCards => developmentCards.Items;

    public BankTradeManager()
    {
        SetupResourceCards();
        SetupDevelopmentCards();
    }

    public bool HasResourceCards(Dictionary<ResourceType, int> resourceCards)
    {
        return this.resourceCards.Has(resourceCards);
    }

    public void AddResourceCards(Dictionary<ResourceType, int> resourceCards)
    {
        foreach (var resourceCard in resourceCards)
        {
            AddResourceCard(resourceCard.Key, resourceCard.Value);
        }
    }

    public void AddResourceCard(ResourceType resourceType, int count)
    {
        resourceCards.Add(resourceType, count);
    }

    public Dictionary<ResourceType, int> RemoveResourceCards(
        Dictionary<ResourceType, int> resourceCards)
    {
        var removedResources = new Dictionary<ResourceType, int>();

        foreach (var resourceCard in resourceCards)
        {
            var remainingCards = this.resourceCards.Count(resourceCard.Key);
            var cardsToRemove = Math.Min(remainingCards, resourceCard.Value);

            if (cardsToRemove == 0)
            {
                continue;
            }

            var removeResult = RemoveResourceCard(resourceCard.Key, cardsToRemove);

            if (removeResult.IsSuccess)
            {
                removedResources.Add(resourceCard.Key, cardsToRemove);
            }
        }

        return removedResources;
    }

    public Result RemoveResourceCard(ResourceType resourceType, int count)
    {
        return resourceCards.Remove(resourceType, count);
    }

    public DevelopmentCardType? RemoveRandomDevelopmentCard()
    {
        return developmentCards.RemoveRandom();
    }

    public Result Trade(
        Player player,
        ResourceType offeredResource,
        ResourceType requestedResource)
    {
        var twoToOneResult = TradeTwoToOne(player, offeredResource, requestedResource);

        if (twoToOneResult.IsSuccess)
        {
            return twoToOneResult;
        }

        var threeToOneResult = TradeThreeToOne(player, offeredResource, requestedResource);

        if (threeToOneResult.IsSuccess)
        {
            return threeToOneResult;
        }

        return Trade(player, offeredResource, requestedResource, 4);
    }

    private void SetupResourceCards()
    {
        resourceCards.Add(ResourceType.Brick, 19);
        resourceCards.Add(ResourceType.Ore, 19);
        resourceCards.Add(ResourceType.Sheep, 19);
        resourceCards.Add(ResourceType.Wheat, 19);
        resourceCards.Add(ResourceType.Wood, 19);
    }

    private void SetupDevelopmentCards()
    {
        developmentCards.Add(DevelopmentCardType.Knight, 14);
        developmentCards.Add(DevelopmentCardType.VictoryPoint, 5);
        developmentCards.Add(DevelopmentCardType.Monopoly, 2);
        developmentCards.Add(DevelopmentCardType.RoadBuilding, 2);
        developmentCards.Add(DevelopmentCardType.YearOfPlenty, 2);
    }

    private Result TradeTwoToOne(
        Player player,
        ResourceType offeredResource,
        ResourceType requestedResource)
    {
        var portTypeValid = Enum.TryParse<PortType>(offeredResource.ToString(), out var portType);

        if (!portTypeValid)
        {
            throw new InvalidOperationException($"Invalid port type: '{offeredResource}'");
        }

        var hasPort = player.HasPort(portType);

        if (!hasPort)
        {
            return Result.Failure(PlayerErrors.DoesNotOwnPort);
        }

        return Trade(player, offeredResource, requestedResource, 2);
    }

    private Result TradeThreeToOne(
        Player player,
        ResourceType offeredResource,
        ResourceType requestedResource)
    {
        var hasPort = player.HasPort(PortType.ThreeToOne);

        if (!hasPort)
        {
            return Result.Failure(PlayerErrors.DoesNotOwnPort);
        }

        return Trade(player, offeredResource, requestedResource, 3);
    }

    private Result Trade(
        Player player,
        ResourceType offeredResource,
        ResourceType requestedResource,
        int offeredResourceCount)
    {
        var hasOfferedResource = player.HasResourceCard(offeredResource, offeredResourceCount);

        if (!hasOfferedResource.IsSuccess)
        {
            return Result.Failure(PlayerErrors.MissingResources);
        }

        var hasRequestedResource = resourceCards.Has(requestedResource);

        if (!hasRequestedResource)
        {
            return Result.Failure(GameErrors.InsufficientResources);
        }

        player.RemoveResourceCard(offeredResource, offeredResourceCount);

        resourceCards.Add(offeredResource, offeredResourceCount);
        resourceCards.Remove(requestedResource);

        player.AddResourceCard(requestedResource);

        return Result.Success();
    }
}
