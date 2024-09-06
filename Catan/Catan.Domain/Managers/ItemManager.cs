using Catan.Domain.Errors;

namespace Catan.Domain.Managers;

public class ItemManager<TItem> where TItem : Enum
{
    private static readonly Random random = new();

    protected readonly Dictionary<TItem, int> items = [];

    public Dictionary<TItem, int> Items => items;

    public bool Has(TItem item, int count = 1)
    {
        return Count(item) >= count;
    }

    public bool Has(Dictionary<TItem, int> items)
    {
        foreach (var item in items)
        {
            if (!Has(item.Key, item.Value))
            {
                return false;
            }
        }

        return true;
    }

    public int Count(TItem item)
    {
        return items.TryGetValue(item, out int value) ? value : 0;
    }

    public int CountAll()
    {
        return items.Values.Sum();
    }

    public void Add(TItem item, int count = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0, nameof(count));

        if (items.TryGetValue(item, out int value))
        {
            items[item] = value + count;
        }
        else
        {
            items[item] = count;
        }
    }

    public void Add(Dictionary<TItem, int> items)
    {
        foreach (var item in items)
        {
            Add(item.Key, item.Value);
        }
    }

    public void Set(TItem item, int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0, nameof(count));

        items[item] = count;
    }

    public Result Remove(TItem item, int count = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0, nameof(count));

        if (items.TryGetValue(item, out int value))
        {
            if (value >= count)
            {
                items[item] = value - count;
                return Result.Success();
            }
            else
            {
                return Result.Failure(ItemManagerErrors.NotEnough);
            }
        }
        else
        {
            return Result.Failure(ItemManagerErrors.NotFound);
        }
    }

    public Result Remove(Dictionary<TItem, int> items)
    {
        foreach (var item in items)
        {
            if (Count(item.Key) < item.Value)
            {
                return Result.Failure(ItemManagerErrors.NotEnough);
            }
        }

        foreach (var item in items)
        {
            Remove(item.Key, item.Value);
        }

        return Result.Success();
    }

    public TItem? RemoveRandom()
    {
        var availableTypes = items
            .Where(x => x.Value > 0)
            .Select(x => x.Key)
            .ToList();

        if (availableTypes.Count == 0)
        {
            return default;
        }

        var randomIndex = random.Next(availableTypes.Count);

        var randomType = availableTypes[randomIndex];

        Remove(randomType);

        return randomType;
    }
}
