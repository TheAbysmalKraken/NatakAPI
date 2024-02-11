using System.Text.Json.Serialization;

namespace Catan.API.Controllers.Models;

public sealed class EmbargoPlayerRequest
{
    [JsonPropertyName("playerColourToEmbargo")]
    public int? PlayerColourToEmbargo { get; set; }
}
