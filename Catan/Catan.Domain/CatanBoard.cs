using static Catan.Common.Enumerations;

namespace Catan.Domain
{
    internal sealed class CatanBoard
    {
        private int boardSize = 3;

        private readonly List<List<List<CatanTile>>> tiles = new List<List<List<CatanTile>>>();
        private readonly List<CatanPort> ports = new List<CatanPort>();
        private List<List<CatanBuilding>> settlementsAndCities = new List<List<CatanBuilding>>();
        private List<List<CatanBuilding>> roads = new List<List<CatanBuilding>>();
        private Coordinates robberPosition;

        private readonly Random random = new();

        public CatanBoard()
        {

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
                { 8, 2 },
                { 9, 2 },
                { 10, 2 },
                { 11, 2 },
                { 12, 1 }
            };

            int maxIndex = (boardSize * 2) - 1;

            for (int i = 0; i < maxIndex; i++)
            {
                for (int j = 0; j < maxIndex; j++)
                {
                    for (int k = 0; k < maxIndex; k++)
                    {
                        tiles[i][j][k] = CreateNewCatanTile(remainingResourceTiles, remainingActivationNumbers);
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

            int tileType;
            int lowestTileTypeNum = (int)remainingResourceTiles.First().Key;
            int highestTileTypeNum = (int)remainingResourceTiles.Last().Key;
            CatanResourceType catanTileType;

            tileType = random.Next(lowestTileTypeNum, highestTileTypeNum + 1);

            catanTileType = (CatanResourceType)tileType;

            remainingResourceTiles[catanTileType]--;

            if (remainingResourceTiles[catanTileType] == 0)
            {
                remainingResourceTiles.Remove(catanTileType);
            }

            int activationNumber;
            int lowestActivationNum = remainingActivationNumbers.First().Key;
            int highestActivationNum = remainingActivationNumbers.Last().Key;

            activationNumber = random.Next(lowestActivationNum, highestActivationNum + 1);

            remainingActivationNumbers[activationNumber]--;

            if (remainingActivationNumbers[activationNumber] == 0)
            {
                remainingActivationNumbers.Remove(activationNumber);
            }

            return new CatanTile(catanTileType, activationNumber);
        }
    }
}
