using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Domain.Purchases;

public sealed class VillagePurchase : Purchase
{
    protected override Dictionary<ResourceType, int> Cost => new()
    {
        { ResourceType.Wood, 1 },
        { ResourceType.Clay, 1 },
        { ResourceType.Animal, 1 },
        { ResourceType.Food, 1 }
    };

    protected override Result MovePurchasedItem(Player player, BankTradeManager bankTradeManager)
    {
        return Result.Success();
    }
}
