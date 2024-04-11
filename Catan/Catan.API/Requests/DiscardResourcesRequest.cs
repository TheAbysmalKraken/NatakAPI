using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class DiscardResourcesRequest
{
    [JsonPropertyName("resources")]
    public Dictionary<int, int>? Resources { get; set; }
}
