using System.Text.Json.Serialization;

namespace Catan.Application.Models;

public sealed class PointResponse
{
    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }
}
