using System.Text.Json.Serialization;

namespace Catan.Core.Features.CreateGame;

public sealed class CreateGameResponse
{
    [JsonPropertyName("gameId")]
    public required string GameId { get; init; }
}
