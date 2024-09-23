using System.Net;

namespace Natak.Domain.Errors;

public static class BoardErrors
{
    public static Error InvalidTilePoint => new(
        HttpStatusCode.BadRequest,
        "Board.InvalidTilePoint",
        "Invalid tile point.");

    public static Error ThiefAlreadyAtLocation => new(
        HttpStatusCode.BadRequest,
        "Board.ThiefAlreadyAtLocation",
        "Thief is already at the specified location.");

    public static Error InvalidRoadPoints => new(
        HttpStatusCode.BadRequest,
        "Board.InvalidRoadPoints",
        "Invalid road points.");

    public static Error RoadNotFound => new(
        HttpStatusCode.BadRequest,
        "Board.RoadNotFound",
        "Road not found at the specified points.");

    public static Error RoadAlreadyExists => new(
        HttpStatusCode.BadRequest,
        "Board.RoadAlreadyExists",
        "Road already exists at the specified points.");

    public static Error RoadIsBlocked => new(
        HttpStatusCode.BadRequest,
        "Board.RoadIsBlocked",
        "Road is blocked by another player's village or town.");

    public static Error RoadDoesNotConnect => new(
        HttpStatusCode.BadRequest,
        "Board.RoadDoesNotConnect",
        "Road does not connect to any existing roads, villages or cities.");

    public static Error InvalidVillagePoint => new(
        HttpStatusCode.BadRequest,
        "Board.InvalidVillagePoint",
        "Invalid village point.");

    public static Error VillageAlreadyExists => new(
        HttpStatusCode.BadRequest,
        "Board.VillageAlreadyExists",
        "Village already exists at the specified point.");

    public static Error VillageIsTooClose => new(
        HttpStatusCode.BadRequest,
        "Board.VillageIsTooClose",
        "Village is too close to another village.");

    public static Error VillageDoesNotConnect => new(
        HttpStatusCode.BadRequest,
        "Board.VillageDoesNotConnect",
        "Village does not connect to any existing roads.");

    public static Error VillageNotOwnedByPlayer => new(
        HttpStatusCode.BadRequest,
        "Board.VillageNotOwnedByPlayer",
        "Village is not owned by the player.");

    public static Error VillageAlreadyUpgraded => new(
        HttpStatusCode.BadRequest,
        "Board.VillageAlreadyUpgraded",
        "Village is already upgraded to a town.");

    public static Error RoadAlreadyConnected => new(
        HttpStatusCode.BadRequest,
        "Board.RoadAlreadyConnected",
        "Road is already connected to the specified village.");
}
