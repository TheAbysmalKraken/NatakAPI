using Catan.Domain.Enums;

namespace Catan.API.Requests;

public sealed class MakeTradeOfferRequest
{
    public required Dictionary<ResourceType, int> Offer { get; init; }
    public required Dictionary<ResourceType, int> Request { get; init; }
}
