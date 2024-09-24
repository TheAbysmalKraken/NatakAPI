using Natak.Domain.Enums;

namespace Natak.Domain;

public static class DomainConstants
{
    public static Dictionary<ResourceType, int> GetTileResourceTypeTotals()
        => new()
        {
            { ResourceType.None, 1 },
            { ResourceType.Wood, 4 },
            { ResourceType.Clay, 3 },
            { ResourceType.Animal, 4 },
            { ResourceType.Food, 4 },
            { ResourceType.Metal, 3 }
        };

    public static Dictionary<int, int> GetTileActivationNumberTotals()
        => new()
        {
            { 2, 1 },
            { 3, 2 },
            { 4, 2 },
            { 5, 2 },
            { 6, 2 },
            { 7, 0 },
            { 8, 2 },
            { 9, 2 },
            { 10, 2 },
            { 11, 2 },
            { 12, 1 }
        };

    public static List<Point> GetStartingPortPoints()
    {
        return [
            new(2, 0),
            new(3, 0),
            new(5, 0),
            new(6, 0),
            new(1, 1),
            new(1, 2),
            new(8, 1),
            new(9, 1),
            new(10, 2),
            new(10, 3),
            new(1, 3),
            new(1, 4),
            new(8, 4),
            new(9, 4),
            new(2, 5),
            new(3, 5),
            new(5, 5),
            new(6, 5)
        ];
    }

    public static Dictionary<PortType, int> GetPortTypeTotals()
        => new()
        {
            { PortType.ThreeToOne, 4 },
            { PortType.Wood, 1 },
            { PortType.Clay, 1 },
            { PortType.Animal, 1 },
            { PortType.Food, 1 },
            { PortType.Metal, 1 }
        };

    public static Dictionary<GrowthCardType, int> GetGrowthCardTypeTotals()
        => new()
        {
            { GrowthCardType.Soldier, 14 },
            { GrowthCardType.Roaming, 2 },
            { GrowthCardType.Wealth, 2 },
            { GrowthCardType.Gatherer, 2 },
            { GrowthCardType.VictoryPoint, 5 }
        };

    public static Dictionary<ResourceType, int> GetBankResourceTotals()
        => new()
        {
            { ResourceType.Wood, 19 },
            { ResourceType.Clay, 19 },
            { ResourceType.Animal, 19 },
            { ResourceType.Food, 19 },
            { ResourceType.Metal, 19 }
        };
}
