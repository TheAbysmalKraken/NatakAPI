using Catan.Domain.Enums;

namespace Catan.Domain.Managers;

public sealed class PlayerPieceManager : ItemManager<BuildingType>
{
    public int Roads => items[BuildingType.Road];

    public int Settlements => items[BuildingType.Settlement];

    public int Cities => items[BuildingType.City];
}
