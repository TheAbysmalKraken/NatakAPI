using Catan.Domain.Enums;
using Catan.Domain.Managers;

namespace Catan.Domain.Purchases;

public sealed class CityPurchase : Purchase
{
    protected override Dictionary<ResourceType, int> Cost => new()
    {
        { ResourceType.Wheat, 2 },
        { ResourceType.Ore, 3 }
    };

    protected override Result MovePurchasedItem(Player player, BankTradeManager bankTradeManager)
    {
        return Result.Success();
    }
}
