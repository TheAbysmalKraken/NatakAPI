using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public sealed class PlayerPieceManagerDto :
    ItemManagerDto<BuildingType>,
    IDto<PlayerPieceManager, PlayerPieceManagerDto>
{
    public static PlayerPieceManagerDto FromDomain(PlayerPieceManager domain)
    {
        return new PlayerPieceManagerDto()
        {
            Items = domain.GetItems()
        };
    }

    public new PlayerPieceManager ToDomain()
    {
        return new PlayerPieceManager(Items);
    }
}