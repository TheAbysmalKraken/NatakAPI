using Catan.Domain.Enums;

namespace Catan.Domain.UnitTests;

public sealed class BoardTests
{
    [Fact]
    public void CreateBoard_EmptyTilesAreNull()
    {
        // Act
        var board = new Board();
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
    public void CreateBoard_CorrectResourceCounts()
    {
        // Arrange
        var resourceCounts = DomainConstants.GetTileResourceTypeTotals();

        // Act
        var board = new Board();
        var tiles = board.GetTiles();

        foreach (var tile in tiles)
        {
            if (tile is null) continue;

            var tileType = tile.Type;

            resourceCounts[tileType]--;
        }

        Assert.Equal(0, resourceCounts[ResourceType.Wood]);
        Assert.Equal(0, resourceCounts[ResourceType.Brick]);
        Assert.Equal(0, resourceCounts[ResourceType.Sheep]);
        Assert.Equal(0, resourceCounts[ResourceType.Wheat]);
        Assert.Equal(0, resourceCounts[ResourceType.Ore]);
        Assert.Equal(0, resourceCounts[ResourceType.Desert]);
    }

    [Fact]
    public void CreateBoard_CorrectActivationNumberCounts()
    {
        // Arrange
        var activationNumberCounts = DomainConstants.GetTileActivationNumberTotals();

        // Act
        var board = new Board();
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
    public void CreateBoard_EmptySettlementsAndCitiesAreNull()
    {
        // Act
        var board = new Board();
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
    public void CreateBoard_IndividualRoadPointsAreAllValid()
    {
        // Act
        var board = new Board();
        var roads = board.GetRoads();

        // Assert
        foreach (var road in roads)
        {
            Assert.NotEqual(new Point(0, 0), road.FirstPoint);
            Assert.NotEqual(new Point(0, 0), road.SecondPoint);
            Assert.NotEqual(new Point(0, 1), road.FirstPoint);
            Assert.NotEqual(new Point(0, 1), road.SecondPoint);
            Assert.NotEqual(new Point(1, 0), road.FirstPoint);
            Assert.NotEqual(new Point(1, 0), road.SecondPoint);
            Assert.NotEqual(new Point(0, 4), road.FirstPoint);
            Assert.NotEqual(new Point(0, 4), road.SecondPoint);
            Assert.NotEqual(new Point(0, 5), road.FirstPoint);
            Assert.NotEqual(new Point(0, 5), road.SecondPoint);
            Assert.NotEqual(new Point(1, 5), road.FirstPoint);
            Assert.NotEqual(new Point(1, 5), road.SecondPoint);
            Assert.NotEqual(new Point(9, 0), road.FirstPoint);
            Assert.NotEqual(new Point(9, 0), road.SecondPoint);
            Assert.NotEqual(new Point(10, 0), road.FirstPoint);
            Assert.NotEqual(new Point(10, 0), road.SecondPoint);
            Assert.NotEqual(new Point(10, 1), road.FirstPoint);
            Assert.NotEqual(new Point(10, 1), road.SecondPoint);
            Assert.NotEqual(new Point(10, 4), road.FirstPoint);
            Assert.NotEqual(new Point(10, 4), road.SecondPoint);
            Assert.NotEqual(new Point(10, 5), road.FirstPoint);
            Assert.NotEqual(new Point(10, 5), road.SecondPoint);
            Assert.NotEqual(new Point(9, 5), road.FirstPoint);
            Assert.NotEqual(new Point(9, 5), road.SecondPoint);
        }

        Assert.Equal(72, roads.Count);
    }

    [Theory]
    [InlineData(3, 0, 3, 1)]
    [InlineData(5, 0, 5, 1)]
    [InlineData(2, 1, 2, 2)]
    [InlineData(4, 1, 4, 2)]
    public void CreateBoard_RoadsDoNotExistAcrossTiles(int x1, int y1, int x2, int y2)
    {
        // Act
        var board = new Board();
        var roads = board.GetRoads();

        // Assert
        var roadInList = roads.FirstOrDefault(r => r.FirstPoint.Equals(new(x1, x2)) && r.SecondPoint.Equals(new(x2, y2)));

        roadInList ??= roads.FirstOrDefault(r => r.FirstPoint.Equals(new(x2, y2)) && r.SecondPoint.Equals(new(x1, y1)));

        Assert.Null(roadInList);
    }

    [Fact]
    public void CreateBoard_CorrectNumberOfPorts()
    {
        // Arrange
        var correctPortLocations = DomainConstants.GetStartingPortPoints();

        // Act
        var board = new Board();
        var ports = board.GetPorts();

        // Assert
        Assert.Equal(correctPortLocations.Count, ports.Count);
    }

    [Fact]
    public void CreateBoard_PortsAreAtCorrectLocations()
    {
        // Arrange
        var correctPortLocations = DomainConstants.GetStartingPortPoints();

        // Act
        var board = new Board();
        var ports = board.GetPorts();

        // Assert
        foreach (var portLocation in correctPortLocations)
        {
            var possiblePorts = ports.Where(x => x.Point.Equals(portLocation));
            Assert.NotEmpty(possiblePorts);
        }
    }

    [Fact]
    public void CreateBoard_PortsAreCorrectTypes()
    {
        // Arrange
        var remainingPortTypeTotals = DomainConstants.GetPortTypeTotals();

        foreach (var type in remainingPortTypeTotals.Keys)
        {
            remainingPortTypeTotals[type] *= 2;
        }

        // Act
        var board = new Board();
        var ports = board.GetPorts();

        // Assert
        foreach (var port in ports)
        {
            if (port is null) continue;

            var tileType = port.Type;

            remainingPortTypeTotals[tileType]--;
        }

        Assert.Equal(0, remainingPortTypeTotals[PortType.Wood]);
        Assert.Equal(0, remainingPortTypeTotals[PortType.Brick]);
        Assert.Equal(0, remainingPortTypeTotals[PortType.Sheep]);
        Assert.Equal(0, remainingPortTypeTotals[PortType.Wheat]);
        Assert.Equal(0, remainingPortTypeTotals[PortType.Ore]);
        Assert.Equal(0, remainingPortTypeTotals[PortType.ThreeToOne]);
    }

    [Fact]
    public void CanPlaceRoadBetweenPoints_ReturnsSuccess()
    {
        // Arrange
        var board = new Board();
        var road = new Road(PlayerColour.None, new Point(0, 2), new Point(0, 3));

        var playerColour = PlayerColour.Blue;

        board.PlaceHouse(road.FirstPoint, playerColour, true);

        // Act
        var result = board.CanPlaceRoadBetweenPoints(road.FirstPoint, road.SecondPoint, playerColour);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void CanPlaceRoadBetweenPoints_OutsideOfBoard_ReturnsFailure()
    {
        // Arrange
        var board = new Board();

        // Act
        var result = board.CanPlaceRoadBetweenPoints(new Point(0, 0), new Point(0, 1), PlayerColour.Blue);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanPlaceRoadBetweenPoints_ColourIsNone_ReturnsFailure()
    {
        // Arrange
        var board = new Board();
        var road = new Road(PlayerColour.None, new Point(0, 2), new Point(0, 3));

        // Act
        var result = board.CanPlaceRoadBetweenPoints(road.FirstPoint, road.SecondPoint, PlayerColour.None);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData(0, 2, 3, 4)]
    [InlineData(0, 2, 1, 3)]
    [InlineData(0, 2, 1, 4)]
    [InlineData(1, 3, 0, 2)]
    [InlineData(3, 1, 5, 1)]
    [InlineData(3, 0, 3, 1)]
    public void CanPlaceRoadBetweenPoints_PointsDoNotConnect_ReturnsFailure(int x1, int y1, int x2, int y2)
    {
        // Arrange
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        board.PlaceHouse(new Point(x1, y1), playerColour, true);

        // Act
        var result = board.CanPlaceRoadBetweenPoints(new Point(x1, y1), new Point(x2, y2), playerColour);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanPlaceRoadBetweenPoints_NoConnectingRoadsOrHouses_ReturnsFailure()
    {
        // Arrange
        var board = new Board();
        var road = new Road(PlayerColour.None, new Point(0, 2), new Point(0, 3));

        var playerColour = PlayerColour.Blue;

        // Act
        var result = board.CanPlaceRoadBetweenPoints(road.FirstPoint, road.SecondPoint, playerColour);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData(PlayerColour.Blue)]
    [InlineData(PlayerColour.Red)]
    [InlineData(PlayerColour.Green)]
    [InlineData(PlayerColour.Yellow)]
    public void CanPlaceRoadBetweenPoints_RoadAlreadyPlaced_ReturnsFailure(PlayerColour colourToPlace)
    {
        // Arrange
        var board = new Board();
        var road = new Road(PlayerColour.None, new Point(0, 2), new Point(0, 3));

        var playerColour = PlayerColour.Blue;

        board.PlaceHouse(road.FirstPoint, playerColour, true);
        board.PlaceRoad(road.FirstPoint, road.SecondPoint, playerColour);

        // Act
        var result = board.CanPlaceRoadBetweenPoints(road.FirstPoint, road.SecondPoint, colourToPlace);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanPlaceRoadBetweenPoints_BlockedByOpposingHouse_ReturnsFailure()
    {
        // Arrange
        var board = new Board();
        var road = new Road(PlayerColour.None, new Point(0, 3), new Point(0, 2));
        var roadConnectedToFirst = new Road(PlayerColour.None, new Point(0, 2), new Point(1, 2));
        var roadConnectedToSecond = new Road(PlayerColour.None, new Point(1, 2), new Point(1, 3));

        var playerColour = PlayerColour.Blue;

        board.PlaceHouse(road.FirstPoint, playerColour, true);
        board.PlaceRoad(road.FirstPoint, road.SecondPoint, playerColour);
        board.PlaceRoad(roadConnectedToFirst.FirstPoint, roadConnectedToFirst.SecondPoint, playerColour);
        board.PlaceHouse(roadConnectedToFirst.SecondPoint, PlayerColour.Red, true);

        // Act
        var result = board.CanPlaceRoadBetweenPoints(
            roadConnectedToSecond.FirstPoint, roadConnectedToSecond.SecondPoint, playerColour);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanPlaceHouseAtPoint_ReturnsSuccess()
    {
        // Arrange
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        // Act
        var result = board.CanPlaceHouseAtPoint(new Point(0, 2), playerColour, true);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 6)]
    [InlineData(11, 0)]
    [InlineData(11, 6)]
    public void CanPlaceHouseAtPoint_OutsideOfBoard_ReturnsFailure(int x, int y)
    {
        // Arrange
        var board = new Board();

        // Act
        var result = board.CanPlaceHouseAtPoint(new Point(x, y), PlayerColour.Blue);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanPlaceHouseAtPoint_ColourIsNone_ReturnsFailure()
    {
        // Arrange
        var board = new Board();

        // Act
        var result = board.CanPlaceHouseAtPoint(new Point(0, 2), PlayerColour.None);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanPlaceHouseAtPoint_PointAlreadyOccupied_ReturnsFailure()
    {
        // Arrange
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        board.PlaceHouse(new Point(0, 2), playerColour, true);

        // Act
        var result = board.CanPlaceHouseAtPoint(new Point(0, 2), playerColour);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanPlaceHouseAtPoint_TooCloseToAnotherHouse_ReturnsFailure()
    {
        // Arrange
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        board.PlaceHouse(new Point(0, 2), playerColour, true);

        // Act
        var result = board.CanPlaceHouseAtPoint(new Point(0, 3), playerColour);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanUpgradeHouseAtPoint_NoHouseAtPoint_ReturnsFailure()
    {
        // Arrange
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        // Act
        var result = board.CanUpgradeHouseAtPoint(new Point(0, 2), playerColour);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanUpgradeHouseAtPoint_HouseIsNotOwnedByPlayer_ReturnsFailure()
    {
        // Arrange
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        board.PlaceHouse(new Point(0, 2), PlayerColour.Red, true);

        // Act
        var result = board.CanUpgradeHouseAtPoint(new Point(0, 2), playerColour);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanUpgradeHouseAtPoint_HouseIsAlreadyACity_ReturnsFailure()
    {
        // Arrange
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        board.PlaceHouse(new Point(0, 2), playerColour, true);
        board.UpgradeHouse(new Point(0, 2), playerColour);

        // Act
        var result = board.CanUpgradeHouseAtPoint(new Point(0, 2), playerColour);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanUpgradeHouseAtPoint_ReturnsSuccess()
    {
        // Arrange
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        board.PlaceHouse(new Point(0, 2), playerColour, true);

        // Act
        var result = board.CanUpgradeHouseAtPoint(new Point(0, 2), playerColour);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void CanMoveRobberToPoint_RobberAlreadyAtPoint_ReturnsFailure()
    {
        // Arrange
        var board = new Board();
        var robberPosition = board.RobberPosition;
        var initialPoint = new Point(1, 1);

        if (!robberPosition.Equals(initialPoint))
        {
            board.MoveRobberToPoint(initialPoint);
        }

        // Act
        var result = board.CanMoveRobberToPoint(initialPoint);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 5)]
    [InlineData(5, 0)]
    [InlineData(5, 5)]
    public void CanMoveRobberToPoint_PointIsInvalid_ReturnsFailure(int x, int y)
    {
        // Arrange
        var board = new Board();

        var point = new Point(x, y);

        // Act
        var result = board.CanMoveRobberToPoint(point);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanMoveRobberToPoint_ReturnsSuccess()
    {
        // Arrange
        var board = new Board();
        var initialPoint = new Point(1, 1);

        if (!board.RobberPosition.Equals(initialPoint))
        {
            board.MoveRobberToPoint(initialPoint);
        }

        var newPoint = new Point(1, 2);

        // Act
        var result = board.CanMoveRobberToPoint(newPoint);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ColourHasPortOfType_ReturnsFalse()
    {
        // Arrange
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        // Act
        var hasPort = board.ColourHasPortOfType(playerColour, PortType.Wood);

        // Assert
        Assert.False(hasPort);
    }

    [Fact]
    public void ColourHasPortOfType_ReturnsTrue()
    {
        // Arrange
        var board = new Board();
        var ports = board.GetPorts();

        var playerColour = PlayerColour.Blue;
        var portType = PortType.Wood;

        var port = ports.First(x => x.Type == portType);

        board.PlaceHouse(port.Point, playerColour, true);

        // Act
        var hasPort = board.ColourHasPortOfType(playerColour, portType);

        // Assert
        Assert.True(hasPort);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsZero()
    {
        // Arrange
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        // Act
        var length = board.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(0, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLength()
    {
        // Arrange
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        var road = new Road(playerColour, new(2, 0), new(3, 0));

        board.PlaceHouse(road.FirstPoint, playerColour, true);
        board.PlaceRoad(road.FirstPoint, road.SecondPoint, playerColour);

        // Act
        var length = board.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(1, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLengthForMultipleRoads()
    {
        // Arrange
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        var roads = new List<Road>
        {
            new(playerColour, new(2, 0), new(3, 0)),
            new(playerColour, new(3, 0), new(4, 0))
        };

        board.PlaceHouse(roads[0].FirstPoint, playerColour, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstPoint, road.SecondPoint, road.Colour);
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
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        var roads = new List<Road>
        {
            new(playerColour, new(2, 0), new(3, 0)),
            new(playerColour, new(3, 0), new(4, 0)),
            new(playerColour, new(5, 0), new(4, 0)),
            new(playerColour, new(5, 0), new(6, 0)),
            new(playerColour, new(4, 0), new(4, 1))
        };

        board.PlaceHouse(roads[0].FirstPoint, playerColour, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstPoint, road.SecondPoint, road.Colour);
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
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        var roads = new List<Road>
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

        board.PlaceHouse(roads[0].FirstPoint, playerColour, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstPoint, road.SecondPoint, road.Colour);
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
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        var roads = new List<Road>
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

        board.PlaceHouse(roads[0].FirstPoint, playerColour, true);
        board.PlaceHouse(roads[17].FirstPoint, playerColour, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstPoint, road.SecondPoint, road.Colour);
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
        var board = new Board();

        var playerColour = PlayerColour.Blue;

        var roads = new List<Road>
        {
            new(playerColour, new(2, 0), new(3, 0)),
            new(playerColour, new(3, 0), new(4, 0)),
            new(playerColour, new(5, 0), new(4, 0)),
            new(playerColour, new(5, 0), new(6, 0)),
            new(playerColour, new(6, 0), new(6, 1)),
            new(playerColour, new(4, 0), new(4, 1))
        };

        board.PlaceHouse(roads[0].FirstPoint, playerColour, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstPoint, road.SecondPoint, road.Colour);
        }

        board.PlaceHouse(roads[1].SecondPoint, PlayerColour.Red, true);

        // Act
        var length = board.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(3, length);
    }

    [Fact]
    public void GetLongestRoadInfo_RoadLessThanFive_ReturnsCorrectColourAndLength()
    {
        // Arrange
        var board = new Board();

        var playerColour1 = PlayerColour.Blue;
        var playerColour2 = PlayerColour.Red;

        var roads = new List<Road>
        {
            new(playerColour1, new(2, 0), new(3, 0)),
            new(playerColour1, new(3, 0), new(4, 0)),
            new(playerColour2, new(3, 1), new(4, 1)),
            new(playerColour2, new(4, 1), new(5, 1)),
            new(playerColour2, new(5, 1), new(5, 2))
        };

        board.PlaceHouse(roads[0].FirstPoint, playerColour1, true);
        board.PlaceHouse(roads[2].FirstPoint, playerColour2, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstPoint, road.SecondPoint, road.Colour);
        }

        // Act
        var longestRoadInfo = board.LongestRoadInfo;

        // Assert
        Assert.Equal(PlayerColour.None, longestRoadInfo.Colour);
        Assert.Equal(0, longestRoadInfo.Length);
    }

    [Fact]
    public void GetLongestRoadInfo_ReturnsCorrectColourAndLength()
    {
        // Arrange
        var board = new Board();

        var playerColour1 = PlayerColour.Blue;
        var playerColour2 = PlayerColour.Red;

        var roads = new List<Road>
        {
            new(playerColour1, new(2, 0), new(3, 0)),
            new(playerColour1, new(3, 0), new(4, 0)),
            new(playerColour2, new(3, 1), new(4, 1)),
            new(playerColour2, new(4, 1), new(5, 1)),
            new(playerColour2, new(5, 1), new(5, 2)),
            new(playerColour2, new(5, 2), new(4, 2)),
            new(playerColour2, new(4, 2), new(3, 2)),
        };

        board.PlaceHouse(roads[0].FirstPoint, playerColour1, true);
        board.PlaceHouse(roads[2].FirstPoint, playerColour2, true);

        foreach (var road in roads)
        {
            board.PlaceRoad(road.FirstPoint, road.SecondPoint, road.Colour);
        }

        // Act
        var longestRoadInfo = board.LongestRoadInfo;

        // Assert
        Assert.Equal(playerColour2, longestRoadInfo.Colour);
        Assert.Equal(5, longestRoadInfo.Length);
    }
}
