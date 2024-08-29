using System.Net;

namespace Catan.Domain.Errors;

public static class GameErrors
{
    public static Error GameNotFound => new(
        HttpStatusCode.NotFound,
        "Game.NotFound",
        "Game not found by specified identifier.");

    public static Error InvalidPlayerCount => new(
        HttpStatusCode.BadRequest,
        "Game.InvalidPlayerCount",
        "Player count must be between 3 and 4.");

    public static Error PlayerToStealFromDoesNotHaveHouseOnTile => new(
        HttpStatusCode.BadRequest,
        "Game.PlayerToStealFromDoesNotHaveHouseOnTile",
        "Player to steal from does not have a house on the specified tile.");

    public static Error InvalidResourceType => new(
        HttpStatusCode.BadRequest,
        "Game.InvalidResourceType",
        "Invalid resource type.");

    public static Error InsufficientResources => new(
        HttpStatusCode.BadRequest,
        "Game.InsufficientResources",
        "Game does not have the required resources to perform this action.");

    public static Error TradeOfferNotActive => new(
        HttpStatusCode.BadRequest,
        "Game.TradeOfferNotActive",
        "Trade offer is not active.");

    public static Error NoDevelopmentCardsLeft => new(
        HttpStatusCode.BadRequest,
        "Game.NoDevelopmentCardsLeft",
        "There are no development cards left in the deck.");
}
