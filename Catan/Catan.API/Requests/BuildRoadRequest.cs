using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class BuildRoadRequest
{
    [JsonPropertyName("firstX")]
    public int? FirstX { get; set; }

    [JsonPropertyName("firstY")]
    public int? FirstY { get; set; }

    [JsonPropertyName("secondX")]
    public int? SecondX { get; set; }

    [JsonPropertyName("secondY")]
    public int? SecondY { get; set; }
}
