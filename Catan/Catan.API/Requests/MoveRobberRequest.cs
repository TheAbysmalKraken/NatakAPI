using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class MoveRobberRequest
{
    [JsonPropertyName("moveRobberTo")]
    public required PointRequest MoveRobberTo { get; init; }
}
