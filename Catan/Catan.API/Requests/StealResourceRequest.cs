using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class StealResourceRequest
{
    [JsonPropertyName("victimColour")]
    public required int VictimColour { get; init; }
}
