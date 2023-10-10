using static Catan.Common.Enumerations;

namespace Catan.Domain
{
    public static class DomainConstants
    {
        public static Dictionary<CatanResourceType, int> GetTileResourceTypeTotals()
        {
            return new Dictionary<CatanResourceType, int>()
            {
                { CatanResourceType.Wood, 4 },
                { CatanResourceType.Brick, 3 },
                { CatanResourceType.Sheep, 4 },
                { CatanResourceType.Wheat, 4 },
                { CatanResourceType.Ore, 3 },
                { CatanResourceType.Desert, 1 }
            };
        }

        public static Dictionary<int, int> GetTileActivationNumberTotals()
        {
            return new Dictionary<int, int>()
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
        }

        public static List<Coordinates> GetStartingPortCoordinates()
        {
            return new List<Coordinates>()
            {
                new Coordinates(2, 0),
                new Coordinates(3, 0),
                new Coordinates(5, 0),
                new Coordinates(6, 0),
                new Coordinates(1, 1),
                new Coordinates(1, 2),
                new Coordinates(8, 1),
                new Coordinates(9, 1),
                new Coordinates(10, 2),
                new Coordinates(10, 3),
                new Coordinates(1, 3),
                new Coordinates(1, 4),
                new Coordinates(8, 4),
                new Coordinates(9, 4),
                new Coordinates(2, 5),
                new Coordinates(3, 5),
                new Coordinates(5, 5),
                new Coordinates(6, 5)
            };
        }

        public static Dictionary<CatanPortType, int> GetPortTypeTotals()
        {
            return new Dictionary<CatanPortType, int>()
            {
                { CatanPortType.None, 0},
                { CatanPortType.Wood, 1 },
                { CatanPortType.Brick, 1 },
                { CatanPortType.Sheep, 1 },
                { CatanPortType.Wheat, 1 },
                { CatanPortType.Ore, 1 },
                { CatanPortType.ThreeToOne, 4 }
            };
        }
    }
}
