using System.Text.Json.Serialization;

namespace Natak.API.Requests;

public sealed class TradeWithBankRequest
{
    [JsonPropertyName("resourceToGive")]
    public required int ResourceToGive { get; init; }

    [JsonPropertyName("resourceToGet")]
    public required int ResourceToGet { get; init; }
}
