using System.Net;

namespace Catan.Domain.Errors;

public static class BoardErrors
{
    public static Error InvalidTilePoint => new(
        HttpStatusCode.BadRequest,
        "Board.InvalidTilePoint",
        "Invalid tile point.");

    public static Error RobberAlreadyAtLocation => new(
        HttpStatusCode.BadRequest,
        "Board.RobberAlreadyAtLocation",
        "Robber is already at the specified location.");

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
        "Road is blocked by another player's settlement or city.");

    public static Error RoadDoesNotConnect => new(
        HttpStatusCode.BadRequest,
        "Board.RoadDoesNotConnect",
        "Road does not connect to any existing roads, settlements or cities.");

    public static Error InvalidSettlementPoint => new(
        HttpStatusCode.BadRequest,
        "Board.InvalidSettlementPoint",
        "Invalid settlement point.");

    public static Error SettlementAlreadyExists => new(
        HttpStatusCode.BadRequest,
        "Board.SettlementAlreadyExists",
        "Settlement already exists at the specified point.");

    public static Error SettlementIsTooClose => new(
        HttpStatusCode.BadRequest,
        "Board.SettlementIsTooClose",
        "Settlement is too close to another settlement.");

    public static Error SettlementDoesNotConnect => new(
        HttpStatusCode.BadRequest,
        "Board.SettlementDoesNotConnect",
        "Settlement does not connect to any existing roads.");

    public static Error SettlementNotOwnedByPlayer => new(
        HttpStatusCode.BadRequest,
        "Board.SettlementNotOwnedByPlayer",
        "Settlement is not owned by the player.");

    public static Error SettlementAlreadyUpgraded => new(
        HttpStatusCode.BadRequest,
        "Board.SettlementAlreadyUpgraded",
        "Settlement is already upgraded to a city.");

    public static Error RoadAlreadyConnected => new(
        HttpStatusCode.BadRequest,
        "Board.RoadAlreadyConnected",
        "Road is already connected to the specified settlement.");
}
