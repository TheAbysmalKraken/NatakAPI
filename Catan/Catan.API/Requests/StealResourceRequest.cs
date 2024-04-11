using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class StealResourceRequest
{
    [JsonPropertyName("playerColourToStealFrom")]
    public int? PlayerColourToStealFrom { get; set; }
}
