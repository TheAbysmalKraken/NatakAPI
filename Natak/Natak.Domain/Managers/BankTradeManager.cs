using Natak.Domain.Enums;
using Natak.Domain.Errors;

namespace Natak.Domain.Managers;

public sealed class BankTradeManager
{
    private readonly ItemManager<ResourceType> resourceCards = new();

    private readonly ItemManager<GrowthCardType> growthCards = new();

    public Dictionary<ResourceType, int> ResourceCards => resourceCards.Items;

    public Dictionary<GrowthCardType, int> GrowthCards => growthCards.Items;

    public BankTradeManager()
    {
        SetupResourceCards();
        SetupGrowthCards();
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

    public GrowthCardType? RemoveRandomGrowthCard()
    {
        return growthCards.RemoveRandom();
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
        resourceCards.Add(ResourceType.Clay, 19);
        resourceCards.Add(ResourceType.Metal, 19);
        resourceCards.Add(ResourceType.Animal, 19);
        resourceCards.Add(ResourceType.Food, 19);
        resourceCards.Add(ResourceType.Wood, 19);
    }

    private void SetupGrowthCards()
    {
        growthCards.Add(GrowthCardType.Soldier, 14);
        growthCards.Add(GrowthCardType.VictoryPoint, 5);
        growthCards.Add(GrowthCardType.Gatherer, 2);
        growthCards.Add(GrowthCardType.Roaming, 2);
        growthCards.Add(GrowthCardType.Wealth, 2);
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
