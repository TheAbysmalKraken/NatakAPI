using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Domain.Purchases;

public sealed class RoadPurchase : Purchase
{
    protected override Dictionary<ResourceType, int> Cost => new()
    {
        { ResourceType.Wood, 1 },
        { ResourceType.Brick, 1 }
    };

    protected override Result MovePurchasedItem(Player player, BankTradeManager bankTradeManager)
    {
        return Result.Success();
    }
}
