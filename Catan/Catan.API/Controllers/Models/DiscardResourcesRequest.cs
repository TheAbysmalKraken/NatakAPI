using System.Text.Json.Serialization;

namespace Catan.API.Controllers.Models;

public sealed class DiscardResourcesRequest
{
    [JsonPropertyName("resources")]
    public Dictionary<int, int>? Resources { get; set; }
}
