using System.Text.Json.Serialization;

namespace Catan.API.Controllers.Models;

public sealed class TradeWithBankRequest
{
    [JsonPropertyName("resourceToGive")]
    public int? ResourceToGive { get; set; }

    [JsonPropertyName("resourceToReceive")]
    public int? ResourceToReceive { get; set; }
}
