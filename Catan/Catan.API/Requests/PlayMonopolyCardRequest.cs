using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class PlayMonopolyCardRequest
{
    [JsonPropertyName("resourceType")]
    public int? ResourceType { get; set; }
}
