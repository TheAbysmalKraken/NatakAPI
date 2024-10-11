using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public sealed class PlayerGrowthCardManagerDto :
    ItemManagerDto<GrowthCardType>,
    IDto<PlayerGrowthCardManager, PlayerGrowthCardManagerDto>
{
    public required Dictionary<GrowthCardType, int> OnHoldCards { get; init; }
    
    public static PlayerGrowthCardManagerDto FromDomain(PlayerGrowthCardManager domain)
    {
        return new PlayerGrowthCardManagerDto()
        {
            OnHoldCards = domain.OnHoldCards,
            Items = domain.Cards
        };
    }

    public new PlayerGrowthCardManager ToDomain()
    {
        return new PlayerGrowthCardManager(
            OnHoldCards,
            Items);
    }
}