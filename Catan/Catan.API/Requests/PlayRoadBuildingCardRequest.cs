using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class PlayRoadBuildingCardRequest
{
    [JsonPropertyName("firstRoadFirstPoint")]
    public required PointRequest FirstRoadFirstPoint { get; init; }

    [JsonPropertyName("firstRoadSecondPoint")]
    public required PointRequest FirstRoadSecondPoint { get; init; }

    [JsonPropertyName("secondRoadFirstPoint")]
    public required PointRequest SecondRoadFirstPoint { get; init; }

    [JsonPropertyName("secondRoadSecondPoint")]
    public required PointRequest SecondRoadSecondPoint { get; init; }
}
