using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class PlayMonopolyCardRequest
{
    [JsonPropertyName("resource")]
    public required int Resource { get; init; }
}
