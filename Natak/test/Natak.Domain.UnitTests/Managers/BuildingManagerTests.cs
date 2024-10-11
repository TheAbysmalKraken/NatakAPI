using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Domain.UnitTests.Managers;

public sealed class BuildingManagerTests
{
    [Fact]
    public void GetHouses_Should_Return54EmptyHouses()
    {
        // Act
        var buildingManager = new BuildingManager();
        var houses = buildingManager.GetHouses();
        var emptyHouses = houses
            .Where(x => x.Type == HouseType.None)
            .ToList();

        // Assert
        Assert.Equal(54, emptyHouses.Count);
    }
    
    [Fact]
    public void GetRoads_Should_ReturnValidIndividualRoadPoints()
    {
        // Act
        var buildingManager = new BuildingManager();
        var roads = buildingManager.GetRoads();

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
    public void GetRoads_ShouldNot_ReturnRoadsAcrossTiles(int x1, int y1, int x2, int y2)
    {
        // Act
        var buildingManager = new BuildingManager();
        var roads = buildingManager.GetRoads();

        // Assert
        var roadInList = roads.FirstOrDefault(r => r.FirstPoint.Equals(new Point(x1, x2)) && r.SecondPoint.Equals(new Point(x2, y2)));

        roadInList ??= roads.FirstOrDefault(r => r.FirstPoint.Equals(new Point(x2, y2)) && r.SecondPoint.Equals(new Point(x1, y1)));

        Assert.Null(roadInList);
    }

    [Fact]
    public void GetRoads_Should_ReturnRoadsBelongingToPlayerColour()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var road = new Road(PlayerColour.Blue, new Point(2, 3), new Point(2, 2));
        buildingManager.AddRoad(road);
        
        // Act
        var blueRoads = buildingManager.GetRoads(PlayerColour.Blue);
        var redRoads = buildingManager.GetRoads(PlayerColour.Red);
        
        // Assert
        Assert.Single(blueRoads);
        Assert.Empty(redRoads);
    }
    
    [Fact]
    public void GetRoadsOfColourAtPoint_Should_ReturnRoadsBelongingToPlayerColour()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var road = new Road(PlayerColour.Blue, new Point(2, 3), new Point(2, 2));
        var redRoad = new Road(PlayerColour.Red, new Point(2, 3), new Point(3, 3));
        buildingManager.AddRoad(road);
        buildingManager.AddRoad(redRoad);
        
        // Act
        var blueRoads = buildingManager.GetRoadsOfColourAtPoint(new Point(2, 3), PlayerColour.Blue);
        var redRoads = buildingManager.GetRoadsOfColourAtPoint(new Point(2, 3), PlayerColour.Red);
        
        // Assert
        Assert.Single(blueRoads);
        Assert.Single(redRoads);
    }
    
    [Fact]
    public void GetOccupiedRoadsAtPoint_Should_ReturnOccupiedRoads()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var road = new Road(PlayerColour.Blue, new Point(2, 3), new Point(2, 2));
        var redRoad = new Road(PlayerColour.Red, new Point(2, 3), new Point(3, 3));
        buildingManager.AddRoad(road);
        buildingManager.AddRoad(redRoad);
        
        // Act
        var occupiedRoads = buildingManager.GetOccupiedRoadsAtPoint(new Point(2, 3));
        var singleRoad = buildingManager.GetOccupiedRoadsAtPoint(new Point(2, 2));
        var unoccupiedRoads = buildingManager.GetOccupiedRoadsAtPoint(new Point(4, 3));
        
        // Assert
        Assert.Equal(2, occupiedRoads.Count);
        Assert.Single(singleRoad);
        Assert.Empty(unoccupiedRoads);
    }
    
    [Fact]
    public void GetHouse_Should_ReturnHouseAtPoint()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var house = new House(PlayerColour.Blue, HouseType.Village, new Point(2, 3));
        buildingManager.AddVillage(house);
        
        // Act
        var village = buildingManager.GetHouse(new Point(2, 3));
        var emptyHouse = buildingManager.GetHouse(new Point(3, 3));
        
        // Assert
        Assert.NotNull(village);
        Assert.Equal(HouseType.Village, village.Type);
        Assert.NotNull(emptyHouse);
        Assert.Equal(HouseType.None, emptyHouse.Type);
    }
    
    [Fact]
    public void GetOccupiedHouse_Should_ReturnOccupiedHouseAtPoint()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var house = new House(PlayerColour.Blue, HouseType.Village, new Point(2, 3));
        buildingManager.AddVillage(house);
        
        // Act
        var village = buildingManager.GetOccupiedHouse(new Point(2, 3));
        var emptyHouse = buildingManager.GetOccupiedHouse(new Point(3, 3));
        
        // Assert
        Assert.NotNull(village);
        Assert.Equal(HouseType.Village, village.Type);
        Assert.Null(emptyHouse);
    }
    
    [Fact]
    public void GetUnoccupiedHousePoints_Should_ReturnUnoccupiedHousePoints()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var house = new House(PlayerColour.Blue, HouseType.Village, new Point(2, 3));
        buildingManager.AddVillage(house);
        
        // Act
        var unoccupiedHousePoints = buildingManager.GetUnoccupiedHousePoints();
        
        // Assert
        Assert.NotEmpty(unoccupiedHousePoints);
        Assert.DoesNotContain(new Point(2, 3), unoccupiedHousePoints);
    }
    
    [Fact]
    public void GetRoad_Should_ReturnRoadBetweenPoints()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var road = new Road(PlayerColour.Blue, new Point(2, 3), new Point(2, 2));
        buildingManager.AddRoad(road);
        
        // Act
        var roadBetweenPoints = buildingManager.GetRoad(new Point(2, 3), new Point(2, 2));
        var roadBetweenPointsReversed = buildingManager.GetRoad(new Point(2, 2), new Point(2, 3));
        var roadBetweenPointsEmpty = buildingManager.GetRoad(new Point(2, 3), new Point(3, 3));
        
        // Assert
        Assert.NotNull(roadBetweenPoints);
        Assert.Equal(road.Colour, roadBetweenPoints.Colour);
        Assert.Equal(road.FirstPoint.X, roadBetweenPoints.FirstPoint.X);
        Assert.Equal(road.FirstPoint.Y, roadBetweenPoints.FirstPoint.Y);
        Assert.Equal(road.SecondPoint.X, roadBetweenPoints.SecondPoint.X);
        Assert.Equal(road.SecondPoint.Y, roadBetweenPoints.SecondPoint.Y);
        Assert.NotNull(roadBetweenPointsReversed);
        Assert.Equal(road.Colour, roadBetweenPointsReversed.Colour);
        Assert.Equal(road.FirstPoint.X, roadBetweenPointsReversed.FirstPoint.X);
        Assert.Equal(road.FirstPoint.Y, roadBetweenPointsReversed.FirstPoint.Y);
        Assert.Equal(road.SecondPoint.X, roadBetweenPointsReversed.SecondPoint.X);
        Assert.Equal(road.SecondPoint.Y, roadBetweenPointsReversed.SecondPoint.Y);
        Assert.NotNull(roadBetweenPointsEmpty);
        Assert.Equal(PlayerColour.None, roadBetweenPointsEmpty.Colour);
    }

    [Fact]
    public void CanAddRoad_Should_ReturnFailure_WhenOutsideOfBoard()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var road = new Road(PlayerColour.Blue, new Point(0, 0), new Point(0, 1));

        // Act
        var result = buildingManager.CanAddRoad(road);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanAddRoad_Should_ReturnFailure_WhenColourIsNone()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var road = new Road(PlayerColour.None, new Point(0, 2), new Point(0, 3));

        // Act
        var result = buildingManager.CanAddRoad(road);

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
    public void CanAddRoad_Should_ReturnFailure_WhenPointsDoNotConnect(int x1, int y1, int x2, int y2)
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var road = new Road(PlayerColour.Blue, new Point(x1, y1), new Point(x2, y2));

        // Act
        var result = buildingManager.CanAddRoad(road);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData(PlayerColour.Blue)]
    [InlineData(PlayerColour.Red)]
    [InlineData(PlayerColour.Green)]
    [InlineData(PlayerColour.Yellow)]
    public void CanAddRoad_Should_ReturnFailure_WhenRoadAlreadyPlaced(PlayerColour colourToPlace)
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var firstRoad = new Road(PlayerColour.Blue, new Point(0, 2), new Point(0, 3));
        var road = new Road(colourToPlace, new Point(0, 2), new Point(0, 3));
        buildingManager.AddRoad(firstRoad);

        // Act
        var result = buildingManager.CanAddRoad(road);

        // Assert
        Assert.True(result.IsFailure);
    }
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 6)]
    [InlineData(11, 0)]
    [InlineData(11, 6)]
    public void CanAddVillage_Should_ReturnFailure_WhenOutsideOfBoard(int x, int y)
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var house = new House(PlayerColour.Blue, HouseType.Village, new Point(x, y));

        // Act
        var result = buildingManager.CanAddVillage(house);

        // Assert
        Assert.True(result.IsFailure);
    }
    
    [Fact]
    public void CanAddVillage_Should_ReturnFailure_WhenColourIsNone()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var house = new House(PlayerColour.None, HouseType.Village, new Point(0, 2));

        // Act
        var result = buildingManager.CanAddVillage(house);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanAddVillage_Should_ReturnFailure_WhenPointAlreadyOccupied()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var house = new House(PlayerColour.Blue, HouseType.Village, new Point(0, 2));

        buildingManager.AddVillage(house);

        // Act
        var result = buildingManager.CanAddVillage(house);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CanAddVillage_Should_ReturnFailure_WhenTooCloseToAnotherHouse()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        const PlayerColour playerColour = PlayerColour.Blue;
        var firstHouse = new House(playerColour, HouseType.Village, new Point(0, 2));
        var secondHouse = new House(playerColour, HouseType.Village, new Point(0, 3));
        buildingManager.AddVillage(firstHouse);

        // Act
        var result = buildingManager.CanAddVillage(secondHouse);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void AddVillage_Should_ReturnSuccess()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var house = new House(PlayerColour.Blue, HouseType.Village, new Point(0, 2));

        // Act
        var result = buildingManager.AddVillage(house);

        // Assert
        var addedHouse = buildingManager.GetHouse(house.Point);
        Assert.True(result.IsSuccess);
        Assert.NotNull(addedHouse);
        Assert.Equal(house.Colour, addedHouse.Colour);
    }
    
    [Fact]
    public void CanUpgradeVillageAtPoint_Should_ReturnFailure_WhenOutsideOfBoard()
    {
        // Arrange
        var buildingManager = new BuildingManager();

        // Act
        var result = buildingManager.CanUpgradeVillageAtPoint(new Point(0, 6));

        // Assert
        Assert.True(result.IsFailure);
    }
    
    [Fact]
    public void CanUpgradeVillageAtPoint_Should_ReturnFailure_WhenHouseIsNone()
    {
        // Arrange
        var buildingManager = new BuildingManager();

        // Act
        var result = buildingManager.CanUpgradeVillageAtPoint(new Point(0, 2));

        // Assert
        Assert.True(result.IsFailure);
    }
    
    [Fact]
    public void CanUpgradeVillageAtPoint_Should_ReturnFailure_WhenHouseHasNoColour()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var house = new House(PlayerColour.None, HouseType.Village, new Point(0, 2));

        buildingManager.AddVillage(house);

        // Act
        var result = buildingManager.CanUpgradeVillageAtPoint(new Point(0, 2));

        // Assert
        Assert.True(result.IsFailure);
    }
    
    [Fact]
    public void CanUpgradeVillageAtPoint_Should_ReturnFailure_WhenHouseIsNotVillage()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var point = new Point(0, 2);
        var house = new House(PlayerColour.Blue, HouseType.Village, point);

        buildingManager.AddVillage(house);
        buildingManager.UpgradeVillageAtPoint(point);

        // Act
        var result = buildingManager.CanUpgradeVillageAtPoint(point);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void UpgradeVillageAtPoint_Should_ReturnSuccess()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        var point = new Point(0, 2);
        var house = new House(PlayerColour.Blue, HouseType.Village, point);

        buildingManager.AddVillage(house);

        // Act
        var result = buildingManager.UpgradeVillageAtPoint(point);

        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void GetLengthOfLongestRoadForColour_Should_ReturnZero_WhenNoRoads()
    {
        // Arrange
        var buildingManager = new BuildingManager();

        // Act
        var length = buildingManager.GetLengthOfLongestRoadForColour(PlayerColour.Blue);

        // Assert
        Assert.Equal(0, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_Should_ReturnCorrectLength()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        const PlayerColour playerColour = PlayerColour.Blue;
        var house = new House(playerColour, HouseType.Village, new Point(2, 0));
        var road = new Road(playerColour, new Point(2, 0), new Point(3, 0));

        buildingManager.AddVillage(house);
        buildingManager.AddRoad(road);

        // Act
        var length = buildingManager.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(1, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLengthForMultipleRoads()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        const PlayerColour playerColour = PlayerColour.Blue;
        var roads = new List<Road>
        {
            new(playerColour, new Point(2, 0), new Point(3, 0)),
            new(playerColour, new Point(3, 0), new Point(4, 0))
        };
        var house = new House(playerColour, HouseType.Village, new Point(2, 0));

        buildingManager.AddVillage(house);

        foreach (var road in roads)
        {
            buildingManager.AddRoad(road);
        }

        // Act
        var length = buildingManager.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(2, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLengthForMultipleRoadsAndBranches()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        const PlayerColour playerColour = PlayerColour.Blue;
        var roads = new List<Road>
        {
            new(playerColour, new Point(2, 0), new Point(3, 0)),
            new(playerColour, new Point(3, 0), new Point(4, 0)),
            new(playerColour, new Point(5, 0), new Point(4, 0)),
            new(playerColour, new Point(5, 0), new Point(6, 0)),
            new(playerColour, new Point(4, 0), new Point(4, 1))
        };
        var house = new House(playerColour, HouseType.Village, new Point(2, 0));

        buildingManager.AddVillage(house);

        foreach (var road in roads)
        {
            buildingManager.AddRoad(road);
        }

        // Act
        var length = buildingManager.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(4, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLengthForMultipleRoadsAndBranchesAndLoops()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        const PlayerColour playerColour = PlayerColour.Blue;
        var roads = new List<Road>
        {
            new(playerColour, new Point(2, 0), new Point(3, 0)),
            new(playerColour, new Point(3, 0), new Point(4, 0)),
            new(playerColour, new Point(5, 0), new Point(4, 0)),
            new(playerColour, new Point(5, 0), new Point(6, 0)),
            new(playerColour, new Point(4, 0), new Point(4, 1)),
            new(playerColour, new Point(4, 1), new Point(5, 1)),
            new(playerColour, new Point(5, 1), new Point(6, 1)),
            new(playerColour, new Point(6, 1), new Point(6, 0)),
            new(playerColour, new Point(6, 0), new Point(7, 0)),
            new(playerColour, new Point(4, 1), new Point(3, 1)),
            new(playerColour, new Point(5, 1), new Point(5, 2)),
            new(playerColour, new Point(5, 2), new Point(4, 2)),
            new(playerColour, new Point(4, 2), new Point(3, 2)),
            new(playerColour, new Point(5, 2), new Point(6, 2)),
            new(playerColour, new Point(6, 2), new Point(7, 2)),
            new(playerColour, new Point(7, 2), new Point(7, 1)),
            new(playerColour, new Point(7, 1), new Point(6, 1))
        };
        var house = new House(playerColour, HouseType.Village, new Point(2, 0));

        buildingManager.AddVillage(house);

        foreach (var road in roads)
        {
            buildingManager.AddRoad(road);
        }

        // Act
        var length = buildingManager.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(12, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLengthForMultipleRoadsAndBranchesAndLoopsAndDisconnectedRoads()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        const PlayerColour playerColour = PlayerColour.Blue;
        var roads = new List<Road>
        {
            new(playerColour, new Point(2, 0), new Point(3, 0)),
            new(playerColour, new Point(3, 0), new Point(4, 0)),
            new(playerColour, new Point(5, 0), new Point(4, 0)),
            new(playerColour, new Point(5, 0), new Point(6, 0)),
            new(playerColour, new Point(4, 0), new Point(4, 1)),
            new(playerColour, new Point(4, 1), new Point(5, 1)),
            new(playerColour, new Point(5, 1), new Point(6, 1)),
            new(playerColour, new Point(6, 1), new Point(6, 0)),
            new(playerColour, new Point(6, 0), new Point(7, 0)),
            new(playerColour, new Point(4, 1), new Point(3, 1)),
            new(playerColour, new Point(5, 1), new Point(5, 2)),
            new(playerColour, new Point(5, 2), new Point(4, 2)),
            new(playerColour, new Point(4, 2), new Point(3, 2)),
            new(playerColour, new Point(5, 2), new Point(6, 2)),
            new(playerColour, new Point(6, 2), new Point(7, 2)),
            new(playerColour, new Point(7, 2), new Point(7, 1)),
            new(playerColour, new Point(7, 1), new Point(6, 1)),
            new(playerColour, new Point(8, 0), new Point(8, 1)),
            new(playerColour, new Point(8, 1), new Point(9, 1)),
            new(playerColour, new Point(9, 1), new Point(9, 2)),
            new(playerColour, new Point(9, 2), new Point(8, 2)),
            new(playerColour, new Point(9, 2), new Point(10, 2))
        };
        var firstHouse = new House(playerColour, HouseType.Village, new Point(2, 0));
        var secondHouse = new House(playerColour, HouseType.Village, roads[17].FirstPoint);

        buildingManager.AddVillage(firstHouse);
        buildingManager.AddVillage(secondHouse);

        foreach (var road in roads)
        {
            buildingManager.AddRoad(road);
        }

        // Act
        var length = buildingManager.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(12, length);
    }

    [Fact]
    public void GetLengthOfLongestRoadForColour_ReturnsCorrectLengthForRoadsBlockedByOpposingHouse()
    {
        // Arrange
        var buildingManager = new BuildingManager();
        const PlayerColour playerColour = PlayerColour.Blue;
        var roads = new List<Road>
        {
            new(playerColour, new Point(2, 0), new Point(3, 0)),
            new(playerColour, new Point(3, 0), new Point(4, 0)),
            new(playerColour, new Point(5, 0), new Point(4, 0)),
            new(playerColour, new Point(5, 0), new Point(6, 0)),
            new(playerColour, new Point(6, 0), new Point(6, 1)),
            new(playerColour, new Point(4, 0), new Point(4, 1))
        };
        var firstHouse = new House(playerColour, HouseType.Village, new Point(2, 0));
        var secondHouse = new House(PlayerColour.Red, HouseType.Village, new Point(4, 0));

        buildingManager.AddVillage(firstHouse);

        foreach (var road in roads)
        {
            buildingManager.AddRoad(road);
        }

        buildingManager.AddVillage(secondHouse);

        // Act
        var length = buildingManager.GetLengthOfLongestRoadForColour(playerColour);

        // Assert
        Assert.Equal(3, length);
    }
}