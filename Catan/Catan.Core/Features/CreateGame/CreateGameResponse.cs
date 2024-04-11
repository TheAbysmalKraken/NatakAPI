using System.Text.Json.Serialization;

namespace Catan.Core;

public sealed class CreateGameResponse
{
    [JsonPropertyName("gameId")]
    public required string GameId { get; init; }
}
