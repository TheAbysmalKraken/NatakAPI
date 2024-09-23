using System.Text.Json.Serialization;

namespace Natak.API.Requests;

public sealed class MoveRobberRequest
{
    [JsonPropertyName("moveRobberTo")]
    public required PointRequest MoveRobberTo { get; init; }
}
