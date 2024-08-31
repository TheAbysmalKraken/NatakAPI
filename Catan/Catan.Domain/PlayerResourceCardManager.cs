using Catan.Domain.Enums;

namespace Catan.Domain;

public sealed class PlayerResourceCardManager : CardManager<ResourceType>
{
    private static readonly Random random = new();

    public ResourceType? RemoveRandom()
    {
        var availableTypes = cards
            .Where(x => x.Value > 0)
            .Select(x => x.Key)
            .ToList();

        if (availableTypes.Count == 0)
        {
            return null;
        }

        var randomIndex = random.Next(availableTypes.Count);

        var randomType = availableTypes[randomIndex];

        Remove(randomType);

        return randomType;
    }
}
