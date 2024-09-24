using Natak.Domain.Enums;
using Natak.Domain.Errors;
using Natak.Domain.Managers;

namespace Natak.Domain.Purchases;

public sealed class GrowthCardPurchase : Purchase
{
    protected override Dictionary<ResourceType, int> Cost => new()
    {
        { ResourceType.Animal, 1 },
        { ResourceType.Food, 1 },
        { ResourceType.Metal, 1 }
    };

    protected override Result MovePurchasedItem(Player player, BankTradeManager bankTradeManager)
    {
        var devCard = bankTradeManager.RemoveRandomGrowthCard();

        if (devCard is null)
        {
            return Result.Failure(GameErrors.NoGrowthCardsLeft);
        }

        player.AddGrowthCard(devCard.Value);

        return Result.Success();
    }
}
