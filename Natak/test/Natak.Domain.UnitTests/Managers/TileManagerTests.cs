using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Domain.UnitTests.Managers;

public sealed class TileManagerTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(4, 3)]
    [InlineData(4, 4)]
    [InlineData(3, 4)]
    public void GetTile_Should_ReturnNull_WhenPointIsInvalid(int x, int y)
    {
        // Arrange
        var tileManager = new TileManager();
        var point = new Point(x, y);
        
        // Act
        var tile = tileManager.GetTile(point);
        
        // Assert
        Assert.Null(tile);
    }
    
    [Fact]
    public void GetTile_Should_ReturnTileAtPoint()
    {
        // Arrange
        var tileManager = new TileManager();
        var point = new Point(2, 2);
        
        // Act
        var tile = tileManager.GetTile(point);
        
        // Assert
        Assert.NotNull(tile);
        Assert.Equal(point.X, tile.Point.X);
        Assert.Equal(point.Y, tile.Point.Y);
    }
    
    [Fact]
    public void GetTiles_Should_ReturnTilesWithActivationNumber()
    {
        // Arrange
        var tileManager = new TileManager();
        
        // Act
        var tiles = tileManager.GetTiles(4);
        
        // Assert
        Assert.NotEmpty(tiles);
        
        foreach (var tile in tiles)
        {
            Assert.Equal(4, tile.ActivationNumber);
        }
    }
    
    [Fact]
    public void GetTiles_Should_ReturnTilesWithType()
    {
        // Arrange
        var tileManager = new TileManager();
        
        // Act
        var tiles = tileManager.GetTiles(ResourceType.Wood);
        
        // Assert
        Assert.NotEmpty(tiles);
        
        foreach (var tile in tiles)
        {
            Assert.Equal(ResourceType.Wood, tile.Type);
        }
    }
    
    [Fact]
    public void GetTiles_Should_HaveCorrectResourceCounts()
    {
        // Arrange
        var resourceCounts = DomainConstants.GetTileResourceTypeTotals();

        // Act
        var tileManager = new TileManager();
        var tiles = tileManager.GetTiles();
        
        foreach (var tile in tiles)
        {
            var tileType = tile.Type;

            resourceCounts[tileType]--;
        }

        Assert.Equal(0, resourceCounts[ResourceType.Wood]);
        Assert.Equal(0, resourceCounts[ResourceType.Clay]);
        Assert.Equal(0, resourceCounts[ResourceType.Animal]);
        Assert.Equal(0, resourceCounts[ResourceType.Food]);
        Assert.Equal(0, resourceCounts[ResourceType.Metal]);
        Assert.Equal(0, resourceCounts[ResourceType.None]);
    }
    
    [Fact]
    public void GetTiles_Should_HaveCorrectActivationNumberCounts()
    {
        // Arrange
        var activationNumberCounts = DomainConstants.GetTileActivationNumberTotals();

        // Act
        var tileManager = new TileManager();
        var tiles = tileManager.GetTiles();

        foreach (var tile in tiles)
        {
            var tileNumber = tile.ActivationNumber;

            if (tileNumber == 0) continue;

            activationNumberCounts[tileNumber]--;
        }

        Assert.Equal(0, activationNumberCounts[2]);
        Assert.Equal(0, activationNumberCounts[3]);
        Assert.Equal(0, activationNumberCounts[4]);
        Assert.Equal(0, activationNumberCounts[5]);
        Assert.Equal(0, activationNumberCounts[6]);
        Assert.Equal(0, activationNumberCounts[7]);
        Assert.Equal(0, activationNumberCounts[8]);
        Assert.Equal(0, activationNumberCounts[9]);
        Assert.Equal(0, activationNumberCounts[10]);
        Assert.Equal(0, activationNumberCounts[11]);
        Assert.Equal(0, activationNumberCounts[12]);
    }
}