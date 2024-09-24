using System.Text.Json.Serialization;

namespace Natak.API.Requests;

public sealed class MoveThiefRequest
{
    [JsonPropertyName("moveThiefTo")]
    public required PointRequest MoveThiefTo { get; init; }
}
