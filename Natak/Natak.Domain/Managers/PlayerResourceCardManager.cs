using Natak.Domain.Enums;

namespace Natak.Domain.Managers;

public sealed class PlayerResourceCardManager : ItemManager<ResourceType>
{
    public PlayerResourceCardManager()
        : base()
    {
    }
    
    public PlayerResourceCardManager(
        Dictionary<ResourceType, int> cards)
        : base(cards)
    {
    }
    
    public Dictionary<ResourceType, int> Cards => items;
}
