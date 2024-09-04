using Catan.Domain.Enums;
using Catan.Domain.Managers;
using Catan.Domain.Purchases;

namespace Catan.Domain;

public static class PurchaseHelper
{
    private static readonly DevelopmentCardPurchase developmentCardPurchase = new();
    private static readonly RoadPurchase roadPurchase = new();
    private static readonly SettlementPurchase settlementPurchase = new();
    private static readonly CityPurchase cityPurchase = new();

    public static Result BuyDevelopmentCard(
        Player player,
        BankTradeManager bankTradeManager)
    {
        return Buy(player, bankTradeManager, developmentCardPurchase);
    }

    public static Result BuyRoad(
        Player player,
        BankTradeManager bankTradeManager)
    {
        return Buy(player, bankTradeManager, roadPurchase);
    }

    public static Result BuySettlement(
        Player player,
        BankTradeManager bankTradeManager)
    {
        return Buy(player, bankTradeManager, settlementPurchase);
    }

    public static Result BuyCity(
        Player player,
        BankTradeManager bankTradeManager)
    {
        return Buy(player, bankTradeManager, cityPurchase);
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
