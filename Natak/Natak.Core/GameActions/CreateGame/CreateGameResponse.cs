using System.Text.Json.Serialization;

namespace Natak.Core.GameActions.CreateGame;

public sealed class CreateGameResponse
{
    [JsonPropertyName("gameId")]
    public required string GameId { get; init; }
}
