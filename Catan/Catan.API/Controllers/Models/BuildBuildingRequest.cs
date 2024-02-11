using System.Text.Json.Serialization;

namespace Catan.API.Controllers.Models;

public sealed class BuildBuildingRequest
{
    [JsonPropertyName("x")]
    public int? X { get; set; }

    [JsonPropertyName("y")]
    public int? Y { get; set; }
}
