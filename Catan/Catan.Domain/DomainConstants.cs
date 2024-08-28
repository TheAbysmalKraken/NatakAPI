using Catan.Domain.Enums;

namespace Catan.Domain;

public static class DomainConstants
{
    public static Dictionary<ResourceType, int> GetTileResourceTypeTotals()
        => new()
        {
            { ResourceType.Desert, 1 },
            { ResourceType.Wood, 4 },
            { ResourceType.Brick, 3 },
            { ResourceType.Sheep, 4 },
            { ResourceType.Wheat, 4 },
            { ResourceType.Ore, 3 }
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
            { PortType.Brick, 1 },
            { PortType.Sheep, 1 },
            { PortType.Wheat, 1 },
            { PortType.Ore, 1 }
        };

    public static Dictionary<DevelopmentCardType, int> GetDevelopmentCardTypeTotals()
        => new()
        {
            { DevelopmentCardType.Knight, 14 },
            { DevelopmentCardType.RoadBuilding, 2 },
            { DevelopmentCardType.YearOfPlenty, 2 },
            { DevelopmentCardType.Monopoly, 2 },
            { DevelopmentCardType.VictoryPoint, 5 }
        };

    public static Dictionary<ResourceType, int> GetBankResourceTotals()
        => new()
        {
            { ResourceType.Wood, 19 },
            { ResourceType.Brick, 19 },
            { ResourceType.Sheep, 19 },
            { ResourceType.Wheat, 19 },
            { ResourceType.Ore, 19 }
        };
}
