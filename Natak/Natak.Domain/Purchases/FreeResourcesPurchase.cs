using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Domain.Purchases;

public sealed class FreeResourcesPurchase(Dictionary<ResourceType, int> resources) : Purchase
{
    protected override Dictionary<ResourceType, int> Cost => [];

    protected override Result MovePurchasedItem(Player player, BankTradeManager bankTradeManager)
    {
        var removedCards = bankTradeManager.RemoveResourceCards(resources);

        player.AddResourceCards(removedCards);

        return Result.Success();
    }
}
