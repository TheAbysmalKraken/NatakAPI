using System.Text.Json.Serialization;

namespace Catan.API.Controllers.Models;

public sealed class PlayRoadBuildingCardRequest
{
    [JsonPropertyName("firstX")]
    public int? FirstX { get; set; }

    [JsonPropertyName("firstY")]
    public int? FirstY { get; set; }

    [JsonPropertyName("secondX")]
    public int? SecondX { get; set; }

    [JsonPropertyName("secondY")]
    public int? SecondY { get; set; }

    [JsonPropertyName("thirdX")]
    public int? ThirdX { get; set; }

    [JsonPropertyName("thirdY")]
    public int? ThirdY { get; set; }

    [JsonPropertyName("fourthX")]
    public int? FourthX { get; set; }

    [JsonPropertyName("fourthY")]
    public int? FourthY { get; set; }
}
