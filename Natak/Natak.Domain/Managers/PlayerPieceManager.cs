using Natak.Domain.Enums;

namespace Natak.Domain.Managers;

public sealed class PlayerPieceManager : ItemManager<BuildingType>
{
    public PlayerPieceManager()
    {
    }
    
    public PlayerPieceManager(Dictionary<BuildingType, int> items)
        : base(items)
    {
    }
    
    public int Roads => items[BuildingType.Road];

    public int Villages => items[BuildingType.Village];

    public int Towns => items[BuildingType.Town];
}
