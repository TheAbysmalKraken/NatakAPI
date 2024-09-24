using System.Text.Json.Serialization;

namespace Natak.API.Requests;

internal sealed class CreateNewGameRequest
{
    [JsonPropertyName("playerCount")]
    public required int PlayerCount { get; init; }

    [JsonPropertyName("seed")]
    public int? Seed { get; init; }
}
