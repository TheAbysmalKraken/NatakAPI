using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class TradeWithBankRequest
{
    [JsonPropertyName("resourceToGive")]
    public int? ResourceToGive { get; set; }

    [JsonPropertyName("resourceToReceive")]
    public int? ResourceToReceive { get; set; }
}
