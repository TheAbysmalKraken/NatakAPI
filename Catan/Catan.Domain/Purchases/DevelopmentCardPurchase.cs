using Catan.Domain.Enums;
using Catan.Domain.Errors;
using Catan.Domain.Managers;

namespace Catan.Domain.Purchases;

public sealed class DevelopmentCardPurchase : Purchase
{
    protected override Dictionary<ResourceType, int> Cost => new()
    {
        { ResourceType.Sheep, 1 },
        { ResourceType.Wheat, 1 },
        { ResourceType.Ore, 1 }
    };

    protected override Result MovePurchasedItem(Player player, BankTradeManager bankTradeManager)
    {
        var devCard = bankTradeManager.RemoveRandomDevelopmentCard();

        if (devCard is null)
        {
            return Result.Failure(GameErrors.NoDevelopmentCardsLeft);
        }

        player.AddDevelopmentCard(devCard.Value);

        return Result.Success();
    }
}