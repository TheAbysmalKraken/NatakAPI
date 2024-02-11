using System.Text.Json.Serialization;

namespace Catan.API.Controllers.Models;

public sealed class PlayMonopolyCardRequest
{
    [JsonPropertyName("resourceType")]
    public int? ResourceType { get; set; }
}
