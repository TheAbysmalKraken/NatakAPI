using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class CreateNewGameRequest
{
    [JsonPropertyName("playerCount")]
    public required int PlayerCount { get; init; }

    [JsonPropertyName("seed")]
    public int? Seed { get; init; }
}
