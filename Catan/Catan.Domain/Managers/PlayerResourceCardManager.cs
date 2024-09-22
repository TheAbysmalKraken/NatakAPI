using Catan.Domain.Enums;

namespace Catan.Domain.Managers;

public sealed class PlayerResourceCardManager : ItemManager<ResourceType>
{
    public Dictionary<ResourceType, int> Cards => items;
}
