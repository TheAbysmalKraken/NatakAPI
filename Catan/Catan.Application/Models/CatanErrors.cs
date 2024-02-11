using System.Net;

namespace Catan.Application.Models;

public static class CatanErrors
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
}
