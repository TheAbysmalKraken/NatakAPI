using System.Text.Json.Serialization;

namespace Catan.API.Controllers.Models;

public sealed class StealResourceRequest
{
    [JsonPropertyName("playerColourToStealFrom")]
    public int? PlayerColourToStealFrom { get; set; }
}
