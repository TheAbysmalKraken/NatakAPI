using System.Text.Json.Serialization;

namespace Catan.API.Requests;

internal sealed class BuildBuildingRequest
{
    [JsonPropertyName("point")]
    public required PointRequest Point { get; init; }
}
