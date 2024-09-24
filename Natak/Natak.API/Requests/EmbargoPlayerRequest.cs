using System.Text.Json.Serialization;

namespace Natak.API.Requests;

public sealed class EmbargoPlayerRequest
{
    [JsonPropertyName("playerColourToEmbargo")]
    public required int PlayerColourToEmbargo { get; init; }
}
