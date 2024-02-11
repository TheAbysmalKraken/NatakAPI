using System.Text.Json.Serialization;

namespace Catan.Application.Models;

public sealed class CoordinatesResponse
{
    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }
}
