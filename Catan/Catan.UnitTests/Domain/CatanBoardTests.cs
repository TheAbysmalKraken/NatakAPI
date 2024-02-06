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

    [Theory]
    [InlineData(3, 0, 3, 1)]
    [InlineData(5, 0, 5, 1)]
    [InlineData(2, 1, 2, 2)]
    [InlineData(4, 1, 4, 2)]
    public void CreateCatanBoard_RoadsDoNotExistAcrossTiles(int x1, int y1, int x2, int y2)
    {
        // Act
        var board = new CatanBoard();
        var roads = board.GetRoads();

        // Assert
        var roadInList = roads.FirstOrDefault(r => r.FirstCornerCoordinates.Equals(new(x1, x2)) && r.SecondCornerCoordinates.Equals(new(x2, y2)));

        roadInList ??= roads.FirstOrDefault(r => r.FirstCornerCoordinates.Equals(new(x2, y2)) && r.SecondCornerCoordinates.Equals(new(x1, y1)));

        Assert.Null(roadInList);
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

        board.PlaceHouse(road.FirstCornerCoordinates, playerColour, true);

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

    [Theory]
    [InlineData(0, 2, 3, 4)]
    [InlineData(0, 2, 1, 3)]
    [InlineData(0, 2, 1, 4)]
    [InlineData(1, 3, 0, 2)]
    [InlineData(3, 1, 5, 1)]
    [InlineData(3, 0, 3, 1)]
    public void CanPlaceRoadBetweenCoordinates_CoordinatesDoNotConnect_ReturnsFalse(int x1, int y1, int x2, int y2)
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        board.PlaceHouse(new Coordinates(x1, y1), playerColour, true);

        // Act
        var canPlaceRoad = board.CanPlaceRoadBetweenCoordinates(new Coordinates(x1, y1), new Coordinates(x2, y2), playerColour);

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

        board.PlaceHouse(road.FirstCornerCoordinates, playerColour, true);
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

        board.PlaceHouse(road.FirstCornerCoordinates, playerColour, true);
        board.PlaceRoad(road.FirstCornerCoordinates, road.SecondCornerCoordinates, playerColour);
        board.PlaceRoad(roadConnectedToFirst.FirstCornerCoordinates, roadConnectedToFirst.SecondCornerCoordinates, playerColour);
        board.PlaceHouse(roadConnectedToFirst.SecondCornerCoordinates, CatanPlayerColour.Red, true);

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
        var canPlaceHouse = board.CanPlaceHouseAtCoordinates(new Coordinates(0, 2), playerColour, true);

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

        board.PlaceHouse(new Coordinates(0, 2), playerColour, true);

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

        board.PlaceHouse(new Coordinates(0, 2), playerColour, true);

        // Act
        var canPlaceHouse = board.CanPlaceHouseAtCoordinates(new Coordinates(0, 3), playerColour);

        // Assert
        Assert.False(canPlaceHouse);
    }

    [Fact]
    public void CanUpgradeHouseAtCoordinates_NoHouseAtCoordinates_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        // Act
        var canUpgradeHouse = board.CanUpgradeHouseAtCoordinates(new Coordinates(0, 2), playerColour);

        // Assert
        Assert.False(canUpgradeHouse);
    }

    [Fact]
    public void CanUpgradeHouseAtCoordinates_HouseIsNotOwnedByPlayer_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        board.PlaceHouse(new Coordinates(0, 2), CatanPlayerColour.Red, true);

        // Act
        var canUpgradeHouse = board.CanUpgradeHouseAtCoordinates(new Coordinates(0, 2), playerColour);

        // Assert
        Assert.False(canUpgradeHouse);
    }

    [Fact]
    public void CanUpgradeHouseAtCoordinates_HouseIsAlreadyACity_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        board.PlaceHouse(new Coordinates(0, 2), playerColour, true);
        board.UpgradeHouse(new Coordinates(0, 2), playerColour);

        // Act
        var canUpgradeHouse = board.CanUpgradeHouseAtCoordinates(new Coordinates(0, 2), playerColour);

        // Assert
        Assert.False(canUpgradeHouse);
    }

    [Fact]
    public void CanUpgradeHouseAtCoordinates_ReturnsTrue()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        board.PlaceHouse(new Coordinates(0, 2), playerColour, true);

        // Act
        var canUpgradeHouse = board.CanUpgradeHouseAtCoordinates(new Coordinates(0, 2), playerColour);

        // Assert
        Assert.True(canUpgradeHouse);
    }

    [Fact]
    public void CanMoveRobberToCoordinates_RobberAlreadyAtCoordinates_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();
        var robberPosition = board.RobberPosition;
        var initialCoordinates = new Coordinates(1, 1);

        if (!robberPosition.Equals(initialCoordinates))
        {
            board.MoveRobberToCoordinates(initialCoordinates);
        }

        // Act
        var canMoveRobber = board.CanMoveRobberToCoordinates(initialCoordinates);

        // Assert
        Assert.False(canMoveRobber);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 5)]
    [InlineData(5, 0)]
    [InlineData(5, 5)]
    public void CanMoveRobberToCoordinates_CoordinatesAreInvalid_ReturnsFalse(int x, int y)
    {
        // Arrange
        var board = new CatanBoard();

        var coordinates = new Coordinates(x, y);

        // Act
        var canMoveRobber = board.CanMoveRobberToCoordinates(coordinates);

        // Assert
        Assert.False(canMoveRobber);
    }

    [Fact]
    public void CanMoveRobberToCoordinates_ReturnsTrue()
    {
        // Arrange
        var board = new CatanBoard();
        var initialCoordinates = new Coordinates(1, 1);

        if (!board.RobberPosition.Equals(initialCoordinates))
        {
            board.MoveRobberToCoordinates(initialCoordinates);
        }

        var newCoordinates = new Coordinates(1, 2);

        // Act
        var canMoveRobber = board.CanMoveRobberToCoordinates(newCoordinates);

        // Assert
        Assert.True(canMoveRobber);
    }

    [Fact]
    public void ColourHasPortOfType_ReturnsFalse()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        // Act
        var hasPort = board.ColourHasPortOfType(playerColour, CatanPortType.Wood);

        // Assert
        Assert.False(hasPort);
    }

    [Fact]
    public void ColourHasPortOfType_ReturnsTrue()
    {
        // Arrange
        var board = new CatanBoard();
        var ports = board.GetPorts();

        var playerColour = CatanPlayerColour.Blue;
        var portType = CatanPortType.Wood;

        var port = ports.First(x => x.Type == portType);

        board.PlaceHouse(port.Coordinates, playerColour, true);

        // Act
        var hasPort = board.ColourHasPortOfType(playerColour, portType);

        // Assert
        Assert.True(hasPort);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsZero()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        // Act
        var length = board.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(0, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLength()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        var road = new CatanRoad(playerColour, new(2, 0), new(3, 0));

        board.PlaceHouse(road.FirstCornerCoordinates, playerColour, true);
        board.PlaceRoad(road.FirstCornerCoordinates, road.SecondCornerCoordinates, playerColour);

        // Act
        var length = board.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(1, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLengthForMultipleRoads()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        var roads = new List<CatanRoad>
        {
            new(playerColour, new(2, 0), new(3, 0)),
            new(playerColour, new(3, 0), new(4, 0))
        };

        board.PlaceHouse(roads[0].FirstCornerCoordinates, playerColour, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstCornerCoordinates, road.SecondCornerCoordinates, road.Colour);
        }

        // Act
        var length = board.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(2, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLengthForMultipleRoadsAndBranches()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        var roads = new List<CatanRoad>
        {
            new(playerColour, new(2, 0), new(3, 0)),
            new(playerColour, new(3, 0), new(4, 0)),
            new(playerColour, new(5, 0), new(4, 0)),
            new(playerColour, new(5, 0), new(6, 0)),
            new(playerColour, new(4, 0), new(4, 1))
        };

        board.PlaceHouse(roads[0].FirstCornerCoordinates, playerColour, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstCornerCoordinates, road.SecondCornerCoordinates, road.Colour);
        }

        // Act
        var length = board.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(4, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLengthForMultipleRoadsAndBranchesAndLoops()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        var roads = new List<CatanRoad>
        {
            new(playerColour, new(2, 0), new(3, 0)),
            new(playerColour, new(3, 0), new(4, 0)),
            new(playerColour, new(5, 0), new(4, 0)),
            new(playerColour, new(5, 0), new(6, 0)),
            new(playerColour, new(4, 0), new(4, 1)),
            new(playerColour, new(4, 1), new(5, 1)),
            new(playerColour, new(5, 1), new(6, 1)),
            new(playerColour, new(6, 1), new(6, 0)),
            new(playerColour, new(6, 0), new(7, 0)),
            new(playerColour, new(4, 1), new(3, 1)),
            new(playerColour, new(5, 1), new(5, 2)),
            new(playerColour, new(5, 2), new(4, 2)),
            new(playerColour, new(4, 2), new(3, 2)),
            new(playerColour, new(5, 2), new(6, 2)),
            new(playerColour, new(6, 2), new(7, 2)),
            new(playerColour, new(7, 2), new(7, 1)),
            new(playerColour, new(7, 1), new(6, 1))
        };

        board.PlaceHouse(roads[0].FirstCornerCoordinates, playerColour, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstCornerCoordinates, road.SecondCornerCoordinates, road.Colour);
        }

        // Act
        var length = board.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(12, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLengthForMultipleRoadsAndBranchesAndLoopsAndDisconnectedRoads()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        var roads = new List<CatanRoad>
        {
            new(playerColour, new(2, 0), new(3, 0)),
            new(playerColour, new(3, 0), new(4, 0)),
            new(playerColour, new(5, 0), new(4, 0)),
            new(playerColour, new(5, 0), new(6, 0)),
            new(playerColour, new(4, 0), new(4, 1)),
            new(playerColour, new(4, 1), new(5, 1)),
            new(playerColour, new(5, 1), new(6, 1)),
            new(playerColour, new(6, 1), new(6, 0)),
            new(playerColour, new(6, 0), new(7, 0)),
            new(playerColour, new(4, 1), new(3, 1)),
            new(playerColour, new(5, 1), new(5, 2)),
            new(playerColour, new(5, 2), new(4, 2)),
            new(playerColour, new(4, 2), new(3, 2)),
            new(playerColour, new(5, 2), new(6, 2)),
            new(playerColour, new(6, 2), new(7, 2)),
            new(playerColour, new(7, 2), new(7, 1)),
            new(playerColour, new(7, 1), new(6, 1)),
            new(playerColour, new(8, 0), new(8, 1)),
            new(playerColour, new(8, 1), new(9, 1)),
            new(playerColour, new(9, 1), new(9, 2)),
            new(playerColour, new(9, 2), new(8, 2)),
            new(playerColour, new(9, 2), new(10, 2))
        };

        board.PlaceHouse(roads[0].FirstCornerCoordinates, playerColour, true);
        board.PlaceHouse(roads[17].FirstCornerCoordinates, playerColour, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstCornerCoordinates, road.SecondCornerCoordinates, road.Colour);
        }

        // Act
        var length = board.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(12, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLengthForRoadsBlockedByOpposingHouse()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour = CatanPlayerColour.Blue;

        var roads = new List<CatanRoad>
        {
            new(playerColour, new(2, 0), new(3, 0)),
            new(playerColour, new(3, 0), new(4, 0)),
            new(playerColour, new(5, 0), new(4, 0)),
            new(playerColour, new(5, 0), new(6, 0)),
            new(playerColour, new(6, 0), new(6, 1)),
            new(playerColour, new(4, 0), new(4, 1))
        };

        board.PlaceHouse(roads[0].FirstCornerCoordinates, playerColour, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstCornerCoordinates, road.SecondCornerCoordinates, road.Colour);
        }

        board.PlaceHouse(roads[1].SecondCornerCoordinates, CatanPlayerColour.Red, true);

        // Act
        var length = board.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(3, length);
    }

    [Fact]
    public void GetLongestRoadInfo_RoadLessThanFive_ReturnsCorrectColourAndLength()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour1 = CatanPlayerColour.Blue;
        var playerColour2 = CatanPlayerColour.Red;

        var roads = new List<CatanRoad>
        {
            new(playerColour1, new(2, 0), new(3, 0)),
            new(playerColour1, new(3, 0), new(4, 0)),
            new(playerColour2, new(3, 1), new(4, 1)),
            new(playerColour2, new(4, 1), new(5, 1)),
            new(playerColour2, new(5, 1), new(5, 2))
        };

        board.PlaceHouse(roads[0].FirstCornerCoordinates, playerColour1, true);
        board.PlaceHouse(roads[2].FirstCornerCoordinates, playerColour2, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstCornerCoordinates, road.SecondCornerCoordinates, road.Colour);
        }

        // Act
        var longestRoadInfo = board.GetLongestRoadInfo();

        // Assert
        Assert.Equal(CatanPlayerColour.None, longestRoadInfo.Colour);
        Assert.Equal(0, longestRoadInfo.Length);
    }

    [Fact]
    public void GetLongestRoadInfo_ReturnsCorrectColourAndLength()
    {
        // Arrange
        var board = new CatanBoard();

        var playerColour1 = CatanPlayerColour.Blue;
        var playerColour2 = CatanPlayerColour.Red;

        var roads = new List<CatanRoad>
        {
            new(playerColour1, new(2, 0), new(3, 0)),
            new(playerColour1, new(3, 0), new(4, 0)),
            new(playerColour2, new(3, 1), new(4, 1)),
            new(playerColour2, new(4, 1), new(5, 1)),
            new(playerColour2, new(5, 1), new(5, 2)),
            new(playerColour2, new(5, 2), new(4, 2)),
            new(playerColour2, new(4, 2), new(3, 2)),
        };

        board.PlaceHouse(roads[0].FirstCornerCoordinates, playerColour1, true);
        board.PlaceHouse(roads[2].FirstCornerCoordinates, playerColour2, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstCornerCoordinates, road.SecondCornerCoordinates, road.Colour);
        }

        // Act
        var longestRoadInfo = board.GetLongestRoadInfo();

        // Assert
        Assert.Equal(playerColour2, longestRoadInfo.Colour);
        Assert.Equal(5, longestRoadInfo.Length);
    }
}
