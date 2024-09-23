using System.Net;

namespace Natak.Domain.Errors;

public static class PlayerErrors
{
    public static Error MissingResources => new(
        HttpStatusCode.BadRequest,
        "Player.MissingResources",
        "Player does not have the required resources to perform this action.");

    public static Error DoesNotOwnPort => new(
        HttpStatusCode.BadRequest,
        "Player.DoesNotOwnPort",
        "Player does not own the required port.");

    public static Error CannotEmbargoSelf => new(
        HttpStatusCode.BadRequest,
        "Player.CannotEmbargoSelf",
        "Player cannot embargo themselves.");

    public static Error AlreadyEmbargoed => new(
        HttpStatusCode.BadRequest,
        "Player.AlreadyEmbargoed",
        "Player has already been embargoed.");

    public static Error NotEmbargoed => new(
        HttpStatusCode.BadRequest,
        "Player.NotEmbargoed",
        "Cannot remove embargo from player who is not embargoed.");

    public static Error InvalidPlayerColour => new(
        HttpStatusCode.BadRequest,
        "Player.InvalidPlayerColour",
        "Player colour must be between 1 and 4.");

    public static Error NotFound => new(
        HttpStatusCode.NotFound,
        "Player.NotFound",
        "Player not found by specified identifier.");

    public static Error DevelopmentCardAlreadyPlayed => new(
        HttpStatusCode.BadRequest,
        "Player.DevelopmentCardAlreadyPlayed",
        "Player has already played a development card this turn.");

    public static Error CannotPlayVictoryPointCard => new(
        HttpStatusCode.BadRequest,
        "Player.CannotPlayVictoryPointCard",
        "Player cannot play a victory point card.");

    public static Error NoDevelopmentCardsOfType => new(
        HttpStatusCode.BadRequest,
        "Player.NoDevelopmentCardsOfType",
        "Player does not have any development cards of the specified type.");

    public static Error CannotTradeWithSelf => new(
        HttpStatusCode.BadRequest,
        "Player.CannotTradeWithSelf",
        "Player cannot trade with themselves.");

    public static Error AlreadyRejectedTrade => new(
        HttpStatusCode.BadRequest,
        "Player.AlreadyRejectedTrade",
        "Player has already rejected the trade.");

    public static Error Embargoed => new(
        HttpStatusCode.BadRequest,
        "Player.Embargoed",
        "A player is embargoed and refuses to trade.");

    public static Error CannotStealFromSelf => new(
        HttpStatusCode.BadRequest,
        "Player.CannotStealFromSelf",
        "Player cannot steal from themselves.");

    public static Error IncorrectDiscardCount => new(
        HttpStatusCode.BadRequest,
        "Player.IncorrectDiscardCount",
        "Player has not discarded the correct number of resources.");
}
