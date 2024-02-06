using static Catan.Common.Enumerations;

namespace Catan.Domain;

public static class DomainConstants
{
    public static Dictionary<CatanResourceType, int> GetTileResourceTypeTotals()
        => new()
        {
            { CatanResourceType.Wood, 4 },
            { CatanResourceType.Brick, 3 },
            { CatanResourceType.Sheep, 4 },
            { CatanResourceType.Wheat, 4 },
            { CatanResourceType.Ore, 3 },
            { CatanResourceType.Desert, 1 }
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

    public static List<Coordinates> GetStartingPortCoordinates()
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

    public static Dictionary<CatanPortType, int> GetPortTypeTotals()
        => new()
        {
            { CatanPortType.None, 0},
            { CatanPortType.Wood, 1 },
            { CatanPortType.Brick, 1 },
            { CatanPortType.Sheep, 1 },
            { CatanPortType.Wheat, 1 },
            { CatanPortType.Ore, 1 },
            { CatanPortType.ThreeToOne, 4 }
        };

    public static Dictionary<CatanDevelopmentCardType, int> GetDevelopmentCardTypeTotals()
        => new()
        {
            { CatanDevelopmentCardType.Knight, 14 },
            { CatanDevelopmentCardType.RoadBuilding, 2 },
            { CatanDevelopmentCardType.YearOfPlenty, 2 },
            { CatanDevelopmentCardType.Monopoly, 2 },
            { CatanDevelopmentCardType.VictoryPoint, 5 }
        };

    public static Dictionary<CatanResourceType, int> GetBankResourceTotals()
        => new()
        {
            { CatanResourceType.Wood, 19 },
            { CatanResourceType.Brick, 19 },
            { CatanResourceType.Sheep, 19 },
            { CatanResourceType.Wheat, 19 },
            { CatanResourceType.Ore, 19 }
        };
}
