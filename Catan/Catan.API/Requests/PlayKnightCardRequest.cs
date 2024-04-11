using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class PlayKnightCardRequest
{
    [JsonPropertyName("x")]
    public int? X { get; set; }

    [JsonPropertyName("y")]
    public int? Y { get; set; }

    [JsonPropertyName("playerColourToStealFrom")]
    public int? PlayerColourToStealFrom { get; set; }
}
