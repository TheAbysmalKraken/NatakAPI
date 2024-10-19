using Natak.Domain.Enums;

namespace Natak.Domain.UnitTests;

public sealed class BoardTests
{
    [Fact]
    public void CanPlaceRoadBetweenPoints_ReturnsSuccess()
    {
        // Arrange
        var board = new Board();
        var road = new Road(PlayerColour.None, new Point(0, 2), new Point(0, 3));

        var playerColour = PlayerColour.Blue;

        board.PlaceVillage(road.FirstPoint, playerColour, true);

        // Act
        var result = board.CanPlaceRoadBetweenPoints(road.FirstPoint, road.SecondPoint, playerColour);

        // Assert
        Assert.True(result.IsSuccess);
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
    
    [Fact]
    public void CanPlaceRoadBetweenPoints_BlockedByOpposingHouse_ReturnsFailure()
    {
        // Arrange
        var board = new Board();
        var road = new Road(PlayerColour.None, new Point(0, 3), new Point(0, 2));
        var roadConnectedToFirst = new Road(PlayerColour.None, new Point(0, 2), new Point(1, 2));
        var roadConnectedToSecond = new Road(PlayerColour.None, new Point(1, 2), new Point(1, 3));

        var playerColour = PlayerColour.Blue;

        board.PlaceVillage(road.FirstPoint, playerColour, true);
        board.PlaceRoad(road.FirstPoint, road.SecondPoint, playerColour);
        board.PlaceRoad(roadConnectedToFirst.FirstPoint, roadConnectedToFirst.SecondPoint, playerColour);
        board.PlaceVillage(roadConnectedToFirst.SecondPoint, PlayerColour.Red, true);

        // Act
        var result = board.CanPlaceRoadBetweenPoints(
            roadConnectedToSecond.FirstPoint, roadConnectedToSecond.SecondPoint, playerColour);

        // Assert
        Assert.True(result.IsFailure);
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
        Assert.Equal(0, remainingPortTypeTotals[PortType.Clay]);
        Assert.Equal(0, remainingPortTypeTotals[PortType.Animal]);
        Assert.Equal(0, remainingPortTypeTotals[PortType.Food]);
        Assert.Equal(0, remainingPortTypeTotals[PortType.Metal]);
        Assert.Equal(0, remainingPortTypeTotals[PortType.ThreeToOne]);
    }

    [Fact]
    public void CanMoveThiefToPoint_ThiefAlreadyAtPoint_ReturnsFailure()
    {
        // Arrange
        var board = new Board();
        var thiefPosition = board.ThiefPosition;
        var initialPoint = new Point(1, 1);

        if (!thiefPosition.Equals(initialPoint))
        {
            board.MoveThiefToPoint(initialPoint);
        }

        // Act
        var result = board.CanMoveThiefToPoint(initialPoint);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 5)]
    [InlineData(5, 0)]
    [InlineData(5, 5)]
    public void CanMoveThiefToPoint_PointIsInvalid_ReturnsFailure(int x, int y)
    {
        // Arrange
        var board = new Board();

        var point = new Point(x, y);

        // Act
        var result = board.CanMoveThiefToPoint(point);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanMoveThiefToPoint_ReturnsSuccess()
    {
        // Arrange
        var board = new Board();
        var initialPoint = new Point(1, 1);

        if (!board.ThiefPosition.Equals(initialPoint))
        {
            board.MoveThiefToPoint(initialPoint);
        }

        var newPoint = new Point(1, 2);

        // Act
        var result = board.CanMoveThiefToPoint(newPoint);

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

        board.PlaceVillage(port.Point, playerColour, true);

        // Act
        var hasPort = board.ColourHasPortOfType(playerColour, portType);

        // Assert
        Assert.True(hasPort);
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

        board.PlaceVillage(roads[0].FirstPoint, playerColour1, true);
        board.PlaceVillage(roads[2].FirstPoint, playerColour2, true);

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

        board.PlaceVillage(roads[0].FirstPoint, playerColour1, true);
        board.PlaceVillage(roads[2].FirstPoint, playerColour2, true);

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
