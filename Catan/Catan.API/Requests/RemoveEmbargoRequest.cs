using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class RemoveEmbargoRequest
{
    [JsonPropertyName("playerColourToRemoveEmbargoOn")]
    public required int PlayerColourToRemoveEmbargoOn { get; init; }
}
