using static Catan.Common.Enumerations;

namespace Catan.Domain
{
    public sealed class CatanBoard
    {
        public Coordinates RobberPosition { get; set; }

        public int BoardLength { get; set; } = 5;

        private readonly CatanTile[,] tiles;
        private readonly List<CatanPort> ports = new List<CatanPort>();
        private CatanBuilding[,] houses;
        private List<CatanRoad> roads = new List<CatanRoad>();

        private readonly Random random = new();

        public CatanBoard()
        {
            tiles = new CatanTile[BoardLength, BoardLength];
            houses = new CatanBuilding[11, 6];

            InitialiseTilesAndSetRobber();
            InitialiseHouses();
            InitialiseRoads();
        }

        public CatanTile[,] GetTiles()
        { 
            return tiles;
        }

        public CatanTile GetTile(int x, int y)
        {
            return tiles[x, y];
        }

        public CatanBuilding[,] GetHouses()
        {
            return houses;
        }

        public CatanBuilding GetHouse(int x, int y)
        {
            return houses[x, y];
        }

        public List<CatanRoad> GetRoads()
        {
            return roads;
        }

        private void InitialiseTilesAndSetRobber()
        {
            var remainingResourceTiles = new Dictionary<CatanResourceType, int>()
            {
                { CatanResourceType.Wood, 4 },
                { CatanResourceType.Brick, 3 },
                { CatanResourceType.Sheep, 4 },
                { CatanResourceType.Wheat, 4 },
                { CatanResourceType.Ore, 3 },
                { CatanResourceType.Desert, 1 }
            };

            var remainingActivationNumbers = new Dictionary<int, int>()
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

            for (int i = 0; i < BoardLength; i++)
            {
                for (int j = 0; j < BoardLength; j++)
                {
                    if (i + j >= 2 && i + j <= BoardLength + 1)
                    {
                        tiles[i, j] = CreateNewCatanTile(remainingResourceTiles, remainingActivationNumbers);

                        if (tiles[i, j].Type == CatanResourceType.Desert)
                        {
                            RobberPosition = new Coordinates(i, j);
                        }
                    }
                }
            }
        }

        private void InitialiseHouses()
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (i + j >= 2 && i + j <= 13 && j - i <= 3 && j - i >= -8)
                    {
                        houses[i, j] = new CatanBuilding(CatanPlayerColour.None, CatanBuildingType.None);
                    }
                }
            }
        }

        private void InitialiseRoads()
        {
            List<Coordinates> roadVectors = new()
            {
                new Coordinates(0, -1),
                new Coordinates(-1, 0)
            };

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    var house = houses[i, j];

                    if (house is null) continue;

                    var roadCorner1 = new Coordinates(i, j);

                    foreach (var vector in roadVectors)
                    {
                        var roadCorner2 = Coordinates.Add(vector, roadCorner1);

                        if (roadCorner2.X < 0 || roadCorner2.Y < 0) continue;

                        var road = new CatanRoad(CatanPlayerColour.None, roadCorner1, roadCorner2);

                        if (houses[roadCorner2.X, roadCorner2.Y] is null) continue;

                        if (Math.Min(roadCorner1.Y, roadCorner2.Y) % 2 == roadCorner1.X % 2 || vector.Y == 0)
                        {
                            roads.Add(road);
                        }
                    }
                }
            }
        }

        private CatanTile CreateNewCatanTile(Dictionary<CatanResourceType, int> remainingResourceTiles, Dictionary<int, int> remainingActivationNumbers)
        {
            if (remainingResourceTiles is null || remainingResourceTiles.Count == 0)
            {
                throw new ArgumentException($"{nameof(remainingResourceTiles)} must not be null or empty.");
            }

            if (remainingActivationNumbers is null || remainingActivationNumbers.Count == 0)
            {
                throw new ArgumentException($"{nameof(remainingActivationNumbers)} must not be null or empty.");
            }

            CatanResourceType catanTileType;
            int lowestTileTypeNum = (int)remainingResourceTiles.First().Key;
            int highestTileTypeNum = (int)remainingResourceTiles.Last().Key;

            do
            {
                catanTileType = (CatanResourceType)random.Next(lowestTileTypeNum, highestTileTypeNum + 1);
            }
            while (remainingResourceTiles[catanTileType] <= 0);

            remainingResourceTiles[catanTileType]--;

            if (catanTileType == CatanResourceType.Desert)
            {
                return new CatanTile(catanTileType, 0);
            }

            int activationNumber;
            int lowestActivationNum = remainingActivationNumbers.First().Key;
            int highestActivationNum = remainingActivationNumbers.Last().Key;

            do
            {
                activationNumber = random.Next(lowestActivationNum, highestActivationNum + 1);
            }
            while (remainingActivationNumbers[activationNumber] <= 0);

            remainingActivationNumbers[activationNumber]--;

            return new CatanTile(catanTileType, activationNumber);
        }
    }
}
