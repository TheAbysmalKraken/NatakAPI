using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public class ItemManagerDto<TItem>
    : IDto<ItemManager<TItem>, ItemManagerDto<TItem>>
    where TItem : Enum
{
    public required Dictionary<TItem, int> Items { get; init; }
    
    public static ItemManagerDto<TItem> FromDomain(ItemManager<TItem> domain)
    {
        return new ItemManagerDto<TItem>()
        {
            Items = domain.Items
        };
    }

    public ItemManager<TItem> ToDomain()
    {
        return new ItemManager<TItem>(Items);
    }
}