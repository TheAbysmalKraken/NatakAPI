using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Domain.Purchases;

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
