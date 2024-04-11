using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class EmbargoPlayerRequest
{
    [JsonPropertyName("playerColourToEmbargo")]
    public int? PlayerColourToEmbargo { get; set; }
}
