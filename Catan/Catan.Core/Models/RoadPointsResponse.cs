using System.Text.Json.Serialization;

namespace Catan.Application.Models;

public sealed class RoadPointsResponse
{
    [JsonPropertyName("firstX")]
    public int FirstX { get; set; }

    [JsonPropertyName("firstY")]
    public int FirstY { get; set; }

    [JsonPropertyName("secondX")]
    public int SecondX { get; set; }

    [JsonPropertyName("secondY")]
    public int SecondY { get; set; }
}
