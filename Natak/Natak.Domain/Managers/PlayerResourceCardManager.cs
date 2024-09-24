using Natak.Domain.Enums;

namespace Natak.Domain.Managers;

public sealed class PlayerResourceCardManager : ItemManager<ResourceType>
{
    public Dictionary<ResourceType, int> Cards => items;
}
