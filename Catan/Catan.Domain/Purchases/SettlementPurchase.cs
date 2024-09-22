using Catan.Domain.Enums;
using Catan.Domain.Managers;

namespace Catan.Domain.Purchases;

public sealed class SettlementPurchase : Purchase
{
    protected override Dictionary<ResourceType, int> Cost => new()
    {
        { ResourceType.Wood, 1 },
        { ResourceType.Brick, 1 },
        { ResourceType.Sheep, 1 },
        { ResourceType.Wheat, 1 }
    };

    protected override Result MovePurchasedItem(Player player, BankTradeManager bankTradeManager)
    {
        return Result.Success();
    }
}
