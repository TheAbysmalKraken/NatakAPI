using Catan.Domain;
using static Catan.Common.Enumerations;

namespace Catan.UnitTests.Domain;

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
        var resourceCounts = DomainConstants.GetTileResourceTypeTotals();

        // Act
        var board = new CatanBoard();
        var tiles = board.GetTiles();

        foreach (var tile in tiles)
        {
            if (tile is null) continue;

            var tileType = tile.Type;

            resourceCounts[tileType]--;
        }

        Assert.Equal(0, resourceCounts[CatanResourceType.Wood]);
        Assert.Equal(0, resourceCounts[CatanResourceType.Brick]);
        Assert.Equal(0, resourceCounts[CatanResourceType.Sheep]);
        Assert.Equal(0, resourceCounts[CatanResourceType.Wheat]);
        Assert.Equal(0, resourceCounts[CatanResourceType.Ore]);
        Assert.Equal(0, resourceCounts[CatanResourceType.Desert]);
    }

    [Fact]
    public void CreateCatanBoard_CorrectActivationNumberCounts()
    {
        // Arrange
        var activationNumberCounts = DomainConstants.GetTileActivationNumberTotals();

        // Act
        var board = new CatanBoard();
        var tiles = board.GetTiles();

        foreach (var tile in tiles)
        {
            if (tile is null) continue;

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

    [Fact]
    public void CreateCatanBoard_EmptySettlementsAndCitiesAreNull()
    {
        // Act
        var board = new CatanBoard();
        var houses = board.GetHouses();
        int notNullCount = 0;

        foreach (var house in houses)
        {
            if (house != null) notNullCount++;
        }

        // Assert
        Assert.Null(houses[0, 0]);
        Assert.Null(houses[0, 1]);
        Assert.Null(houses[1, 0]);
        Assert.Null(houses[0, 4]);
        Assert.Null(houses[0, 5]);
        Assert.Null(houses[1, 5]);
        Assert.Null(houses[9, 0]);
        Assert.Null(houses[10, 0]);
        Assert.Null(houses[10, 1]);
        Assert.Null(houses[10, 4]);
        Assert.Null(houses[10, 5]);
        Assert.Null(houses[9, 5]);

        Assert.Equal(54, notNullCount);
    }

    [Fact]
    public void CreateCatanBoard_IndividualRoadCoordinatesAreAllValid()
    {
        // Act
        var board = new CatanBoard();
        var roads = board.GetRoads();

        // Assert
        foreach (var road in roads)
        {
            Assert.NotEqual(new Coordinates(0, 0), road.FirstCornerCoordinates);
            Assert.NotEqual(new Coordinates(0, 0), road.SecondCornerCoordinates);
            Assert.NotEqual(new Coordinates(0, 1), road.FirstCornerCoordinates);
            Assert.NotEqual(new Coordinates(0, 1), road.SecondCornerCoordinates);
            Assert.NotEqual(new Coordinates(1, 0), road.FirstCornerCoordinates);
            Assert.NotEqual(new Coordinates(1, 0), road.SecondCornerCoordinates);
            Assert.NotEqual(new Coordinates(0, 4), road.FirstCornerCoordinates);
            Assert.NotEqual(new Coordinates(0, 4), road.SecondCornerCoordinates);
            Assert.NotEqual(new Coordinates(0, 5), road.FirstCornerCoordinates);
            Assert.NotEqual(new Coordinates(0, 5), road.SecondCornerCoordinates);
            Assert.NotEqual(new Coordinates(1, 5), road.FirstCornerCoordinates);
            Assert.NotEqual(new Coordinates(1, 5), road.SecondCornerCoordinates);
            Assert.NotEqual(new Coordinates(9, 0), road.FirstCornerCoordinates);
            Assert.NotEqual(new Coordinates(9, 0), road.SecondCornerCoordinates);
            Assert.NotEqual(new Coordinates(10, 0), road.FirstCornerCoordinates);
            Assert.NotEqual(new Coordinates(10, 0), road.SecondCornerCoordinates);
            Assert.NotEqual(new Coordinates(10, 1), road.FirstCornerCoordinates);
            Assert.NotEqual(new Coordinates(10, 1), road.SecondCornerCoordinates);
            Assert.NotEqual(new Coordinates(10, 4), road.FirstCornerCoordinates);
            Assert.NotEqual(new Coordinates(10, 4), road.SecondCornerCoordinates);
            Assert.NotEqual(new Coordinates(10, 5), road.FirstCornerCoordinates);
            Assert.NotEqual(new Coordinates(10, 5), road.SecondCornerCoordinates);
            Assert.NotEqual(new Coordinates(9, 5), road.FirstCornerCoordinates);
            Assert.NotEqual(new Coordinates(9, 5), road.SecondCornerCoordinates);
        }

        Assert.Equal(72, roads.Count);
    }

    [Fact]
    public void CreateCatanBoard_CorrectNumberOfPorts()
    {
        // Arrange
        var correctPortLocations = DomainConstants.GetStartingPortCoordinates();

        // Act
        var board = new CatanBoard();
        var ports = board.GetPorts();

        // Assert
        Assert.Equal(correctPortLocations.Count, ports.Count);
    }

    [Fact]
    public void CreateCatanBoard_PortsAreAtCorrectLocations()
    {
        // Arrange
        var correctPortLocations = DomainConstants.GetStartingPortCoordinates();

        // Act
        var board = new CatanBoard();
        var ports = board.GetPorts();

        // Assert
        foreach (var portLocation in correctPortLocations)
        {
            var possiblePorts = ports.Where(x => x.Coordinates.Equals(portLocation));
            Assert.NotEmpty(possiblePorts);
        }
    }

    [Fact]
    public void CreateCatanBoard_PortsAreCorrectTypes()
    {
        // Arrange
        var remainingPortTypeTotals = DomainConstants.GetPortTypeTotals();

        foreach (var type in remainingPortTypeTotals.Keys)
        {
            remainingPortTypeTotals[type] *= 2;
        }

        // Act
        var board = new CatanBoard();
        var ports = board.GetPorts();

        // Assert
        foreach (var port in ports)
        {
            if (port is null) continue;

            var tileType = port.Type;

            remainingPortTypeTotals[tileType]--;
        }

        Assert.Equal(0, remainingPortTypeTotals[CatanPortType.Wood]);
        Assert.Equal(0, remainingPortTypeTotals[CatanPortType.Brick]);
        Assert.Equal(0, remainingPortTypeTotals[CatanPortType.Sheep]);
        Assert.Equal(0, remainingPortTypeTotals[CatanPortType.Wheat]);
        Assert.Equal(0, remainingPortTypeTotals[CatanPortType.Ore]);
        Assert.Equal(0, remainingPortTypeTotals[CatanPortType.ThreeToOne]);
    }

    [Fact]
    public void CanPlaceRoadBetweenCoordinates_ReturnsTrue()
    {
        // Arrange
        var board = new CatanBoard();
        var road = new CatanRoad(CatanPlayerColour.None, new Coordinates(0, 2), new Coordinates(0, 3));

        var playerColour = CatanPlayerColour.Blue;

        board.PlaceHouse(road.FirstCornerCoordinates, playerColour);

        // Act
        var canPlaceRoad = board.CanPlaceRoadBetweenCoordinates(road.FirstCornerCoordinates, road.SecondCornerCoordinates, playerColour);

        // Assert
        Assert.True(canPlaceRoad);
    }

    [Fact]
    public void CanPlaceRoadBetweenCoordinates_OutsideOfBoard_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();

        // Act
        var canPlaceRoad = board.CanPlaceRoadBetweenCoordinates(new Coordinates(0, 0), new Coordinates(0, 1), CatanPlayerColour.Blue);

        // Assert
        Assert.False(canPlaceRoad);
    }

    [Fact]
    public void CanPlaceRoadBetweenCoordinates_ColourIsNone_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();
        var road = new CatanRoad(CatanPlayerColour.None, new Coordinates(0, 2), new Coordinates(0, 3));

        // Act
        var canPlaceRoad = board.CanPlaceRoadBetweenCoordinates(road.FirstCornerCoordinates, road.SecondCornerCoordinates, CatanPlayerColour.None);

        // Assert
        Assert.False(canPlaceRoad);
    }

    [Fact]
    public void CanPlaceRoadBetweenCoordinates_CoordinatesDoNotConnect_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();

        // Act
        var canPlaceRoad = board.CanPlaceRoadBetweenCoordinates(new Coordinates(0, 2), new Coordinates(3, 4), CatanPlayerColour.Blue);

        // Assert
        Assert.False(canPlaceRoad);
    }

    [Fact]
    public void CanPlaceRoadBetweenCoordinates_NoConnectingRoadsOrHouses_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();
        var road = new CatanRoad(CatanPlayerColour.None, new Coordinates(0, 2), new Coordinates(0, 3));

        var playerColour = CatanPlayerColour.Blue;

        // Act
        var canPlaceRoad = board.CanPlaceRoadBetweenCoordinates(road.FirstCornerCoordinates, road.SecondCornerCoordinates, playerColour);

        // Assert
        Assert.False(canPlaceRoad);
    }

    [Theory]
    [InlineData(CatanPlayerColour.Blue)]
    [InlineData(CatanPlayerColour.Red)]
    [InlineData(CatanPlayerColour.Green)]
    [InlineData(CatanPlayerColour.Yellow)]
    public void CanPlaceRoadBetweenCoordinates_RoadAlreadyPlaced_ReturnsFalse(CatanPlayerColour colourToPlace)
    {
        // Arrange
        var board = new CatanBoard();
        var road = new CatanRoad(CatanPlayerColour.None, new Coordinates(0, 2), new Coordinates(0, 3));

        var playerColour = CatanPlayerColour.Blue;

        board.PlaceHouse(road.FirstCornerCoordinates, playerColour);
        board.PlaceRoad(road.FirstCornerCoordinates, road.SecondCornerCoordinates, playerColour);

        // Act
        var canPlaceRoad = board.CanPlaceRoadBetweenCoordinates(road.FirstCornerCoordinates, road.SecondCornerCoordinates, colourToPlace);

        // Assert
        Assert.False(canPlaceRoad);
    }

    [Fact]
    public void CanPlaceRoadBetweenCoordinates_BlockedByOpposingHouse_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();
        var road = new CatanRoad(CatanPlayerColour.None, new Coordinates(0, 3), new Coordinates(0, 2));
        var roadConnectedToFirst = new CatanRoad(CatanPlayerColour.None, new Coordinates(0, 2), new Coordinates(1, 2));
        var roadConnectedToSecond = new CatanRoad(CatanPlayerColour.None, new Coordinates(1, 2), new Coordinates(1, 3));

        var playerColour = CatanPlayerColour.Blue;

        board.PlaceHouse(road.FirstCornerCoordinates, playerColour);
        board.PlaceRoad(road.FirstCornerCoordinates, road.SecondCornerCoordinates, playerColour);
        board.PlaceRoad(roadConnectedToFirst.FirstCornerCoordinates, roadConnectedToFirst.SecondCornerCoordinates, playerColour);
        board.PlaceHouse(roadConnectedToFirst.SecondCornerCoordinates, CatanPlayerColour.Red);

        // Act
        var canPlaceRoad = board.CanPlaceRoadBetweenCoordinates(
            roadConnectedToSecond.FirstCornerCoordinates, roadConnectedToSecond.SecondCornerCoordinates, playerColour);

        // Assert
        Assert.False(canPlaceRoad);
    }

    [Fact]
    public void CanPlaceHouseAtCoordinates_ReturnsTrue()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        // Act
        var canPlaceHouse = board.CanPlaceHouseAtCoordinates(new Coordinates(0, 2), playerColour);

        // Assert
        Assert.True(canPlaceHouse);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 6)]
    [InlineData(11, 0)]
    [InlineData(11, 6)]
    public void CanPlaceHouseAtCoordinates_OutsideOfBoard_ReturnsFalse(int x, int y)
    {
        // Arrange
        var board = new CatanBoard();

        // Act
        var canPlaceHouse = board.CanPlaceHouseAtCoordinates(new Coordinates(x, y), CatanPlayerColour.Blue);

        // Assert
        Assert.False(canPlaceHouse);
    }

    [Fact]
    public void CanPlaceHouseAtCoordinates_ColourIsNone_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();

        // Act
        var canPlaceHouse = board.CanPlaceHouseAtCoordinates(new Coordinates(0, 2), CatanPlayerColour.None);

        // Assert
        Assert.False(canPlaceHouse);
    }

    [Fact]
    public void CanPlaceHouseAtCoordinates_CoordinatesAlreadyOccupied_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        board.PlaceHouse(new Coordinates(0, 2), playerColour);

        // Act
        var canPlaceHouse = board.CanPlaceHouseAtCoordinates(new Coordinates(0, 2), playerColour);

        // Assert
        Assert.False(canPlaceHouse);
    }

    [Fact]
    public void CanPlaceHouseAtCoordinates_TooCloseToAnotherHouse_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        board.PlaceHouse(new Coordinates(0, 2), playerColour);

        // Act
        var canPlaceHouse = board.CanPlaceHouseAtCoordinates(new Coordinates(0, 3), playerColour);

        // Assert
        Assert.False(canPlaceHouse);
    }
}
