using System.Text.Json.Serialization;
using Natak.Domain;

namespace Natak.Core.Models;

public sealed class TradeOfferResponse
{
    [JsonPropertyName("isActive")]
    public required bool IsActive { get; init; }

    [JsonPropertyName("offer")]
    public required Dictionary<int, int> Offer { get; init; }

    [JsonPropertyName("request")]
    public required Dictionary<int, int> Request { get; init; }

    [JsonPropertyName("rejectedBy")]
    public required List<int> RejectedBy { get; init; }

    public static TradeOfferResponse FromDomain(TradeOffer tradeOffer)
    {
        return new()
        {
            IsActive = tradeOffer.IsActive,
            Offer = tradeOffer.Offer.ToDictionary(x => (int)x.Key, x => x.Value),
            Request = tradeOffer.Request.ToDictionary(x => (int)x.Key, x => x.Value),
            RejectedBy = tradeOffer.RejectedBy.Select(x => (int)x).ToList()
        };
    }
}
