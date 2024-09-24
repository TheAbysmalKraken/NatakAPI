using System.Text.Json.Serialization;

namespace Natak.API.Requests;

public sealed class PlayGathererCardRequest
{
    [JsonPropertyName("resource")]
    public required int Resource { get; init; }
}
