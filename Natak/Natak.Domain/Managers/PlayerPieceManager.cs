using Natak.Domain.Enums;

namespace Natak.Domain.Managers;

public sealed class PlayerPieceManager : ItemManager<BuildingType>
{
    public int Roads => items[BuildingType.Road];

    public int Villages => items[BuildingType.Village];

    public int Cities => items[BuildingType.Town];
}
