using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Domain.Purchases;

public sealed class TownPurchase : Purchase
{
    protected override Dictionary<ResourceType, int> Cost => new()
    {
        { ResourceType.Food, 2 },
        { ResourceType.Metal, 3 }
    };

    protected override Result MovePurchasedItem(Player player, BankTradeManager bankTradeManager)
    {
        return Result.Success();
    }
}
