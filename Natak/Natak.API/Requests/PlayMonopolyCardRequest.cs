using System.Text.Json.Serialization;

namespace Natak.API.Requests;

public sealed class PlayMonopolyCardRequest
{
    [JsonPropertyName("resource")]
    public required int Resource { get; init; }
}
