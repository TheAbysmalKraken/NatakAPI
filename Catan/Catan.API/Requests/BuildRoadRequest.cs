using System.Text.Json.Serialization;

namespace Catan.API.Requests;

internal sealed class BuildRoadRequest
{
    [JsonPropertyName("firstPoint")]
    public required PointRequest FirstPoint { get; init; }

    [JsonPropertyName("secondPoint")]
    public required PointRequest SecondPoint { get; init; }
}
