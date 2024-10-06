using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public sealed class BankTradeManagerDto :
    IDto<BankTradeManager, BankTradeManagerDto>
{
    public required ItemManagerDto<ResourceType> ResourceCards { get; init; }
    
    public required ItemManagerDto<GrowthCardType> GrowthCards { get; init; }
    
    public static BankTradeManagerDto FromDomain(BankTradeManager domain)
    {
        return new BankTradeManagerDto()
        {
            ResourceCards = ItemManagerDto<ResourceType>.FromDomain(
                domain.GetResourceCardManager()),
            GrowthCards = ItemManagerDto<GrowthCardType>.FromDomain(
                domain.GetGrowthCardManager())
        };
    }

    public BankTradeManager ToDomain()
    {
        return new BankTradeManager(
            ResourceCards.ToDomain(),
            GrowthCards.ToDomain());
    }
}