using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public sealed class PlayerTradeManagerDto :
    IDto<PlayerTradeManager, PlayerTradeManagerDto>
{
    public required int PlayerCount { get; init; }
    
    public required TradeOfferDto TradeOffer { get; init; }
    
    public required Dictionary<PlayerColour, List<PlayerColour>> Embargoes { get; init; }
    
    public static PlayerTradeManagerDto FromDomain(PlayerTradeManager domain)
    {
        return new PlayerTradeManagerDto()
        {
            PlayerCount = domain.PlayerCount,
            TradeOffer = TradeOfferDto.FromDomain(domain.TradeOffer),
            Embargoes = domain.Embargoes
        };
    }

    public PlayerTradeManager ToDomain()
    {
        return new PlayerTradeManager(
            PlayerCount,
            TradeOffer.ToDomain(),
            Embargoes);
    }
}