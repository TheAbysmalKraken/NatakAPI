using Natak.Domain;
using Natak.Domain.Enums;

namespace Natak.Infrastructure.DTOs;

public sealed class TradeOfferDto :
    IDto<TradeOffer, TradeOfferDto>
{
    public required bool IsActive { get; init; }
    
    public required PlayerColour? OfferingPlayer { get; init; }
    
    public required Dictionary<ResourceType, int> Offer { get; init; }
    
    public required Dictionary<ResourceType, int> Request { get; init; }
    
    public required List<PlayerColour> RejectedBy { get; init; }
    
    public static TradeOfferDto FromDomain(TradeOffer domain)
    {
        return new TradeOfferDto()
        {
            IsActive = domain.IsActive,
            OfferingPlayer = domain.OfferingPlayer,
            Offer = domain.Offer,
            Request = domain.Request,
            RejectedBy = domain.RejectedBy
        };
    }

    public TradeOffer ToDomain()
    {
        return new TradeOffer()
        {
            IsActive = IsActive,
            OfferingPlayer = OfferingPlayer,
            Offer = Offer,
            Request = Request,
            RejectedBy = RejectedBy
        };
    }
}