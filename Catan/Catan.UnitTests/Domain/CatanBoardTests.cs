using Catan.Domain;
using static Catan.Common.Enumerations;

namespace Catan.UnitTests.Domain
{
    public sealed class CatanBoardTests
    {
        [Fact]
        public void CreateCatanBoard_EmptyTilesAreNull()
        {
            // Act
            var board = new CatanBoard();
            var tiles = board.GetTiles();

            // Assert
            Assert.Null(tiles[0, 0]);
            Assert.Null(tiles[0, 1]);
            Assert.Null(tiles[1, 0]);
            Assert.Null(tiles[4, 3]);
            Assert.Null(tiles[4, 4]);
            Assert.Null(tiles[3, 4]);
        }

        [Fact]
        public void CreateCatanBoard_CorrectResourceCounts()
        {
            // Arrange
            var resourceCounts = new Dictionary<CatanResourceType, int>()
            {
                { CatanResourceType.Wood, 0 },
                { CatanResourceType.Brick, 0 },
                { CatanResourceType.Sheep, 0 },
                { CatanResourceType.Wheat, 0 },
                { CatanResourceType.Ore, 0 },
                { CatanResourceType.Desert, 0 }
            };

            // Act
            var board = new CatanBoard();
            var tiles = board.GetTiles();

            foreach (var tile in tiles)
            {
                if (tile is null) continue;

                var tileType = tile.GetResourceType();

                resourceCounts[tileType]++;
            }

            Assert.Equal(4, resourceCounts[CatanResourceType.Wood]);
            Assert.Equal(3, resourceCounts[CatanResourceType.Brick]);
            Assert.Equal(4, resourceCounts[CatanResourceType.Sheep]);
            Assert.Equal(4, resourceCounts[CatanResourceType.Wheat]);
            Assert.Equal(3, resourceCounts[CatanResourceType.Ore]);
            Assert.Equal(1, resourceCounts[CatanResourceType.Desert]);
        }

        [Fact]
        public void CreateCatanBoard_CorrectActivationNumberCounts()
        {
            // Arrange
            var activationNumberCounts = new Dictionary<int, int>()
            {
                { 2, 0 },
                { 3, 0 },
                { 4, 0 },
                { 5, 0 },
                { 6, 0 },
                { 7, 0 },
                { 8, 0 },
                { 9, 0 },
                { 10, 0 },
                { 11, 0 },
                { 12, 0 }
            };

            // Act
            var board = new CatanBoard();
            var tiles = board.GetTiles();

            foreach (var tile in tiles)
            {
                if (tile is null) continue;

                var tileNumber = tile.GetActivationNumber();

                if (tileNumber == 0) continue;

                activationNumberCounts[tileNumber]++;
            }

            Assert.Equal(1, activationNumberCounts[2]);
            Assert.Equal(2, activationNumberCounts[3]);
            Assert.Equal(2, activationNumberCounts[4]);
            Assert.Equal(2, activationNumberCounts[5]);
            Assert.Equal(2, activationNumberCounts[6]);
            Assert.Equal(0, activationNumberCounts[7]);
            Assert.Equal(2, activationNumberCounts[8]);
            Assert.Equal(2, activationNumberCounts[9]);
            Assert.Equal(2, activationNumberCounts[10]);
            Assert.Equal(2, activationNumberCounts[11]);
            Assert.Equal(1, activationNumberCounts[12]);
        }

        [Fact]
        public void CreateCatanBoard_EmptySettlementsAndCitiesAreNull()
        {
            // Act
            var board = new CatanBoard();
            var settlementsAndCities = board.GetSettlementsAndCities();

            // Assert
            Assert.Null(settlementsAndCities[0, 0]);
            Assert.Null(settlementsAndCities[0, 1]);
            Assert.Null(settlementsAndCities[1, 0]);
            Assert.Null(settlementsAndCities[0, 4]);
            Assert.Null(settlementsAndCities[0, 5]);
            Assert.Null(settlementsAndCities[1, 5]);
            Assert.Null(settlementsAndCities[9, 0]);
            Assert.Null(settlementsAndCities[10, 0]);
            Assert.Null(settlementsAndCities[10, 1]);
            Assert.Null(settlementsAndCities[10, 4]);
            Assert.Null(settlementsAndCities[10, 5]);
            Assert.Null(settlementsAndCities[9, 5]);
        }
    }
}
