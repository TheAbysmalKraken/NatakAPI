using Natak.Domain.Enums;
using Natak.Domain.Managers;
using Natak.Domain.Purchases;

namespace Natak.Domain;

public static class PurchaseHelper
{
    private static readonly GrowthCardPurchase growthCardPurchase = new();
    private static readonly RoadPurchase roadPurchase = new();
    private static readonly VillagePurchase villagePurchase = new();
    private static readonly TownPurchase townPurchase = new();

    public static Result BuyGrowthCard(
        Player player,
        BankTradeManager bankTradeManager)
    {
        return Buy(player, bankTradeManager, growthCardPurchase);
    }

    public static Result BuyRoad(
        Player player,
        BankTradeManager bankTradeManager)
    {
        return Buy(player, bankTradeManager, roadPurchase);
    }

    public static Result BuyVillage(
        Player player,
        BankTradeManager bankTradeManager)
    {
        return Buy(player, bankTradeManager, villagePurchase);
    }

    public static Result BuyTown(
        Player player,
        BankTradeManager bankTradeManager)
    {
        return Buy(player, bankTradeManager, townPurchase);
    }

    public static Result DiscardResources(
        Player player,
        BankTradeManager bankTradeManager,
        Dictionary<ResourceType, int> resources)
    {
        var discardPurchase = new DiscardResourcesPurchase(resources);
        return Buy(player, bankTradeManager, discardPurchase);
    }

    public static Result GetFreeResources(
        Player player,
        BankTradeManager bankTradeManager,
        Dictionary<ResourceType, int> resources)
    {
        var freeResourcesPurchase = new FreeResourcesPurchase(resources);
        return Buy(player, bankTradeManager, freeResourcesPurchase);
    }

    private static Result Buy(
        Player player,
        BankTradeManager bankTradeManager,
        Purchase purchase)
    {
        return purchase.Make(player, bankTradeManager);
    }
}
