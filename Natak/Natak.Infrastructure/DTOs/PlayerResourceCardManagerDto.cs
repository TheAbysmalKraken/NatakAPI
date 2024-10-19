using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public sealed class PlayerResourceCardManagerDto :
    ItemManagerDto<ResourceType>,
    IDto<PlayerResourceCardManager, PlayerResourceCardManagerDto>
{
    public static PlayerResourceCardManagerDto FromDomain(PlayerResourceCardManager domain)
    {
        return new PlayerResourceCardManagerDto()
        {
            Items = domain.GetItems()
        };
    }

    public new PlayerResourceCardManager ToDomain()
    {
        return new PlayerResourceCardManager(Items);
    }
}