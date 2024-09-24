using System.Text.Json.Serialization;

namespace Natak.API.Requests;

public sealed class RemoveEmbargoRequest
{
    [JsonPropertyName("playerColourToRemoveEmbargoOn")]
    public required int PlayerColourToRemoveEmbargoOn { get; init; }
}
