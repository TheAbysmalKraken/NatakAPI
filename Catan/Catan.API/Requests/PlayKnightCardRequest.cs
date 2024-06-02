using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class PlayKnightCardRequest
{
    [JsonPropertyName("x")]
    public required int X { get; init; }

    [JsonPropertyName("y")]
    public required int Y { get; init; }

    [JsonPropertyName("playerColourToStealFrom")]
    public required int PlayerColourToStealFrom { get; init; }
}
