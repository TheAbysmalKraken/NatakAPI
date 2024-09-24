using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Domain.Purchases;

public abstract class Purchase
{
    protected abstract Dictionary<ResourceType, int> Cost { get; }

    public Result Make(Player player, BankTradeManager bankTradeManager)
    {
        var playerHasResources = player.HasResourceCards(Cost);

        if (playerHasResources.IsFailure)
        {
            return playerHasResources;
        }

        var removeResourcesFromPlayer = player.RemoveResourceCards(Cost);

        if (removeResourcesFromPlayer.IsFailure)
        {
            throw new InvalidOperationException("Player should have enough resources to make purchase");
        }

        bankTradeManager.AddResourceCards(Cost);

        return MovePurchasedItem(player, bankTradeManager);
    }

    protected abstract Result MovePurchasedItem(Player player, BankTradeManager bankTradeManager);
}
