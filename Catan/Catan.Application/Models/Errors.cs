using System.Net;

namespace Catan.Application.Models;

public static class Errors
{
    public static readonly Error InvalidPlayerCount = new(
        HttpStatusCode.BadRequest,
        "Catan.InvalidPlayerCount",
        "Player count must be between 3 and 4.");

    public static readonly Error InvalidPlayerColour = new(
        HttpStatusCode.BadRequest,
        "Catan.InvalidPlayerColour",
        "Player colour must be between 0 and 3.");

    public static readonly Error GameNotFound = new(
        HttpStatusCode.NotFound,
        "Catan.GameNotFound",
        "Game not found with specified identifier.");

    public static readonly Error InvalidGamePhase = new(
        HttpStatusCode.BadRequest,
        "Catan.InvalidGamePhase",
        "Game is not in a valid phase for this action.");

    public static readonly Error InvalidBuildLocation = new(
        HttpStatusCode.BadRequest,
        "Catan.InvalidBuildLocation",
        "Invalid location for building.");

    public static readonly Error CannotBuyDevelopmentCard = new(
        HttpStatusCode.BadRequest,
        "Catan.CannotBuyDevelopmentCard",
        "Player cannot buy a development card at this time.");

    public static readonly Error AlreadyPlayedDevelopmentCard = new(
        HttpStatusCode.BadRequest,
        "Catan.AlreadyPlayedDevelopmentCard",
        "Player has already played a development card this turn.");

    public static readonly Error CannotPlayDevelopmentCard = new(
        HttpStatusCode.BadRequest,
        "Catan.CannotPlayDevelopmentCard",
        "Player cannot play the development card at this time.");

    public static readonly Error CannotMoveRobberToLocation = new(
        HttpStatusCode.BadRequest,
        "Catan.CannotMoveRobberToLocation",
        "Player cannot move the robber to the specified location.");

    public static readonly Error CannotStealResource = new(
        HttpStatusCode.BadRequest,
        "Catan.CannotStealResource",
        "Player cannot steal a resource from the specified player.");

    public static readonly Error CannotDiscardResources = new(
        HttpStatusCode.BadRequest,
        "Catan.CannotDiscardResources",
        "Player cannot discard the specified resources at this time.");

    public static readonly Error CannotTradeWithBank = new(
        HttpStatusCode.BadRequest,
        "Catan.CannotTradeWithBank",
        "Player cannot trade with the bank at this time.");

    public static readonly Error CannotEmbargoPlayer = new(
        HttpStatusCode.BadRequest,
        "Catan.CannotEmbargoPlayer",
        "Player cannot embargo the specified player at this time.");

}
