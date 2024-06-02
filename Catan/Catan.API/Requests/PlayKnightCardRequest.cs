using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class PlayKnightCardRequest
{
    [JsonPropertyName("moveRobberTo")]
    public required PointRequest MoveRobberTo { get; init; }

    [JsonPropertyName("playerColourToStealFrom")]
    public required int PlayerColourToStealFrom { get; init; }
}
