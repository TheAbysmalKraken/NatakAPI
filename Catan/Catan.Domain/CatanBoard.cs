using static Catan.Common.Enumerations;

namespace Catan.Domain
{
    public sealed class CatanBoard
    {
        private int boardLength = 5;

        private readonly CatanTile[,] tiles;
        private readonly List<CatanPort> ports = new List<CatanPort>();
        private CatanBuilding[,] settlementsAndCities;
        private List<List<CatanBuilding>> roads = new List<List<CatanBuilding>>();
        private Coordinates robberPosition;

        private readonly Random random = new();

        public CatanBoard()
        {
            tiles = new CatanTile[boardLength, boardLength];
            settlementsAndCities = new CatanBuilding[11, 6];

            InitialiseTiles();
            InitialiseSettlementsAndCities();
        }

        public CatanTile[,] GetTiles()
        { 
            return tiles;
        }

        public CatanTile GetTile(int x, int y)
        {
            return tiles[x, y];
        }

        public CatanBuilding[,] GetSettlementsAndCities()
        {
            return settlementsAndCities;
        }

        public CatanBuilding GetSettlementOrCity(int x, int y)
        {
            return settlementsAndCities[x, y];
        }

        private void InitialiseTiles()
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

            for (int i = 0; i < boardLength; i++)
            {
                for (int j = 0; j < boardLength; j++)
                {
                    if (i + j >= 2 && i + j <= boardLength + 1)
                    {
                        tiles[i, j] = CreateNewCatanTile(remainingResourceTiles, remainingActivationNumbers);
                    }
                }
            }
        }

        private void InitialiseSettlementsAndCities()
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (i + j >= 2 && i + j <= 13 && j - i <= 3 && j - i >= -8)
                    {
                        settlementsAndCities[i, j] = new CatanBuilding(CatanPlayerColour.None, CatanBuildingType.None);
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
