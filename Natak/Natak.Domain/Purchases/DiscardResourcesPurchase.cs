using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Domain.Purchases;

public sealed class DiscardResourcesPurchase(Dictionary<ResourceType, int> resources) : Purchase
{
    protected override Dictionary<ResourceType, int> Cost => resources;

    protected override Result MovePurchasedItem(Player player, BankTradeManager bankTradeManager)
    {
        player.CardsToDiscard -= resources.Values.Sum();

        return Result.Success();
    }
}
