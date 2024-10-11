using Natak.Domain.Enums;
using Natak.Domain.Errors;
using Natak.Domain.Managers;

namespace Natak.Domain;

public sealed class Board
{
    private readonly TileManager tileManager;
    private readonly BuildingManager buildingManager;
    private readonly List<Port> ports;

    private readonly Random random = new();
    
    public Board(
        TileManager tileManager,
        BuildingManager buildingManager,
        List<Port> ports,
        Point thiefPosition,
        LongestRoadInfo longestRoadInfo)
    {
        this.tileManager = tileManager;
        this.buildingManager = buildingManager;
        this.ports = ports;
        ThiefPosition = thiefPosition;
        LongestRoadInfo = longestRoadInfo;
    }

    public Board()
    {
        ports = [];
        ThiefPosition = new Point(0, 0);
        LongestRoadInfo = new(PlayerColour.None, 0);

        tileManager = new TileManager();
        buildingManager = new BuildingManager();
        ThiefPosition = tileManager.GetTiles(ResourceType.None)?.FirstOrDefault()?.Point
            ?? throw new InvalidOperationException("Thief position must not be null.");
        InitialisePorts();
    }

    public Point ThiefPosition { get; private set; }

    public LongestRoadInfo LongestRoadInfo { get; private set; }
    
    public TileManager GetTileManager()
        => tileManager;
    
    public BuildingManager GetBuildingManager()
        => buildingManager;

    public List<Road> GetRoads() => buildingManager.GetRoads();

    public List<Port> GetPorts() => ports;

    public Tile? GetTile(Point point)
        => tileManager.GetTile(point);

    public House? GetHouse(Point point)
        => buildingManager.GetHouse(point);
    
    public Road? GetRoad(Point point1, Point point2)
        => buildingManager.GetRoad(point1, point2);

    public Result<List<Road>> GetAvailableRoadLocations(
        PlayerColour colour,
        bool isSetup = false)
    {
        if (colour == PlayerColour.None)
        {
            return Result.Failure<List<Road>>(PlayerErrors.InvalidPlayerColour);
        }

        var availableRoadPoints = new List<Road>();
        var roads = buildingManager.GetRoads();

        foreach (var road in roads)
        {
            var canPlaceRoadResult = isSetup
                ? CanPlaceSetupRoadBetweenPoints(
                    road.FirstPoint,
                    road.SecondPoint,
                    colour)
                : CanPlaceRoadBetweenPoints(
                    road.FirstPoint,
                    road.SecondPoint,
                    colour);

            if (canPlaceRoadResult.IsSuccess)
            {
                availableRoadPoints.Add(road);
            }
        }

        return Result.Success(availableRoadPoints);
    }

    public Result<List<Point>> GetAvailableVillageLocations(
        PlayerColour colour,
        bool isSetup = false)
    {
        if (colour == PlayerColour.None)
        {
            return Result.Failure<List<Point>>(PlayerErrors.InvalidPlayerColour);
        }
        
        var unoccupiedHousePoints = buildingManager.GetUnoccupiedHousePoints();

        var availableVillagePoints = new List<Point>();

        foreach (var point in unoccupiedHousePoints)
        {
            var village = new House(colour, HouseType.Village, point);

            var canPlaceHouseResult = CanPlaceVillage(
                village,
                isSetup);

            if (canPlaceHouseResult.IsSuccess)
            {
                availableVillagePoints.Add(point);
            }
        }

        return Result.Success(availableVillagePoints);
    }

    public Result<List<Point>> GetAvailableTownLocations(PlayerColour colour)
    {
        if (colour == PlayerColour.None)
        {
            return Result.Failure<List<Point>>(PlayerErrors.InvalidPlayerColour);
        }

        var availableTownPoints = new List<Point>();

        for (var x = 0; x < 11; x++)
        {
            for (var y = 0; y < 6; y++)
            {
                var point = new Point(x, y);

                var canUpgradeResult = CanUpgradeVillageAtPoint(point, colour);

                if (canUpgradeResult.IsSuccess)
                {
                    availableTownPoints.Add(point);
                }
            }
        }

        return Result.Success(availableTownPoints);
    }
    
    public int GetLengthOfLongestRoadForColour(PlayerColour colour)
    {
        return buildingManager.GetLengthOfLongestRoadForColour(colour);
    }

    public bool ColourHasPortOfType(PlayerColour colour, PortType portType)
    {
        return ports.Any(p => p.Type == portType && buildingManager.GetHouse(p.Point)?.Colour == colour);
    }

    public Result CanMoveThiefToPoint(Point point)
    {
        if (!TilePointIsValid(point))
        {
            return Result.Failure(BoardErrors.InvalidTilePoint);
        }

        if (point.Equals(ThiefPosition))
        {
            return Result.Failure(BoardErrors.ThiefAlreadyAtLocation);
        }

        return Result.Success();
    }

    public Result MoveThiefToPoint(Point point)
    {
        var result = CanMoveThiefToPoint(point);

        if (result.IsFailure)
        {
            return result;
        }

        ThiefPosition = point;

        return Result.Success();
    }

    public Result UpgradeVillageAtPoint(Point point, PlayerColour colour)
    {
        var canUpgradeResult = CanUpgradeVillageAtPoint(point, colour);

        if (canUpgradeResult.IsFailure)
        {
            return canUpgradeResult;
        }

        var upgradeResult = buildingManager.UpgradeVillageAtPoint(point);
        
        if (upgradeResult.IsFailure)
        {
            throw new InvalidOperationException($"Cannot upgrade village: {upgradeResult.Error}.");
        }
        
        return Result.Success();
    }

    public Result PlaceVillage(Point point, PlayerColour colour, bool isSetup = false)
    {
        var village = new House(colour, HouseType.Town, point);
        var canPlaceResult = CanPlaceVillage(village, isSetup);

        if (canPlaceResult.IsFailure)
        {
            return canPlaceResult;
        }

        var addResult = buildingManager.AddVillage(village);
        
        if (addResult.IsFailure)
        {
            throw new InvalidOperationException($"Cannot add house: {addResult.Error}.");
        }

        UpdateLongestRoadInfo();

        return Result.Success();
    }

    public Result CanPlaceRoadBetweenPoints(Point point1, Point point2, PlayerColour colour)
    {
        if (colour == PlayerColour.None)
        {
            return Result.Failure(PlayerErrors.InvalidPlayerColour);
        }

        if (!RoadPointsAreValid(point1, point2))
        {
            return Result.Failure(BoardErrors.InvalidRoadPoints);
        }

        if (RoadIsClaimed(point1, point2))
        {
            return Result.Failure(BoardErrors.RoadAlreadyExists);
        }

        if (RoadAtPointsIsBlockedByOpposingHouse(point1, point2, colour))
        {
            return Result.Failure(BoardErrors.RoadIsBlocked);
        }

        if (!RoadIsConnectedToHouseOrRoadOfSameColour(point1, point2, colour))
        {
            return Result.Failure(BoardErrors.RoadDoesNotConnect);
        }

        return Result.Success();
    }

    public Result PlaceRoad(Point point1, Point point2, PlayerColour colour, bool isSetup = false)
    {
        var canPlaceResult = isSetup
            ? CanPlaceSetupRoadBetweenPoints(point1, point2, colour)
            : CanPlaceRoadBetweenPoints(point1, point2, colour);

        if (canPlaceResult.IsFailure)
        {
            return canPlaceResult;
        }
        
        var road = new Road(colour, point1, point2);
        
        var addResult = buildingManager.AddRoad(road);
        
        if (addResult.IsFailure)
        {
            throw new InvalidOperationException($"Cannot add road: {addResult.Error}.");
        }

        UpdateLongestRoadInfo();

        return Result.Success();
    }

    public List<Point> GetPointsOfTilesWithActivationNumber(int activationNumber)
    {
        if (activationNumber < 2 || activationNumber > 12)
        {
            throw new ArgumentException("Activation number must be between 2 and 12.");
        }

        return tileManager.GetTiles(activationNumber).Select(t => t.Point).ToList();
    }

    public List<PlayerColour> GetHouseColoursOnTile(Point point)
    {
        if (!TilePointIsValid(point))
        {
            throw new ArgumentException("Point is not valid.");
        }

        var surroundingHouses = GetHousesOnTile(point);

        var houseColours = new List<PlayerColour>();

        foreach (var house in surroundingHouses)
        {
            houseColours.Add(house.Colour);
        }

        return houseColours;
    }

    public List<House> GetHousesOnTile(Point point)
    {
        if (!TilePointIsValid(point))
        {
            throw new ArgumentException("Point are not valid.");
        }

        var surroundingHousePoints = TileToSurroundingHousePoints(point);

        var housesOnTile = new List<House>();

        foreach (var housePoint in surroundingHousePoints)
        {
            if (!IsHousePointValid(housePoint))
            {
                continue;
            }

            var house = buildingManager.GetOccupiedHouse(housePoint);

            if (house != null)
            {
                housesOnTile.Add(house);
            }
        }

        return housesOnTile;
    }

    public List<Tile> GetTilesSurroundingHouse(Point point)
    {
        if (!IsHousePointValid(point))
        {
            throw new ArgumentException("Point are not valid.");
        }

        var surroundingTilePoints = HouseToSurroundingTilePoints(point);

        var tilesSurroundingHouse = new List<Tile>();

        foreach (var tilePoint in surroundingTilePoints)
        {
            var tile = tileManager.GetTile(tilePoint);

            if (tile != null)
            {
                tilesSurroundingHouse.Add(tile);
            }
        }

        return tilesSurroundingHouse;
    }
    
    private Result CanUpgradeVillageAtPoint(Point point, PlayerColour colour)
    {
        var canUpgradeResult = buildingManager.CanUpgradeVillageAtPoint(point);
        
        if (canUpgradeResult.IsFailure)
        {
            return canUpgradeResult;
        }

        var village = GetHouse(point);
        
        if (village is null)
        {
            throw new InvalidOperationException("Village must not be null.");
        }

        return village.Colour != colour
            ? Result.Failure(BoardErrors.VillageNotOwnedByPlayer)
            : Result.Success();
    }
    
    private Result CanPlaceVillage(House village, bool isSetup = false)
    {
        var canAddVillage = buildingManager.CanAddVillage(village);
        
        if (canAddVillage.IsFailure)
        {
            return canAddVillage;
        }

        if (!isSetup && GetRoadsOfColourConnectedToPoint(village.Point, village.Colour).Count == 0)
        {
            return Result.Failure(BoardErrors.VillageDoesNotConnect);
        }

        return Result.Success();
    }
    
    private Result CanPlaceSetupRoadBetweenPoints(Point point1, Point point2, PlayerColour colour)
    {
        if (colour == PlayerColour.None)
        {
            return Result.Failure(PlayerErrors.InvalidPlayerColour);
        }

        if (!RoadPointsAreValid(point1, point2))
        {
            return Result.Failure(BoardErrors.InvalidRoadPoints);
        }

        if (RoadIsClaimed(point1, point2))
        {
            return Result.Failure(BoardErrors.RoadAlreadyExists);
        }

        var point1ContainsHouseOfSameColour = PointContainsHouseOfColour(point1, colour);
        var point2ContainsHouseOfSameColour = PointContainsHouseOfColour(point2, colour);

        if (!point1ContainsHouseOfSameColour && !point2ContainsHouseOfSameColour)
        {
            return Result.Failure(BoardErrors.RoadDoesNotConnect);
        }

        // If road is connected to a house AND a road at the same point, it is invalid for setup.
        if ((GetRoadsOfColourConnectedToPoint(point1, colour).Count > 0
             && point1ContainsHouseOfSameColour)
            || (GetRoadsOfColourConnectedToPoint(point2, colour).Count > 0
                && point2ContainsHouseOfSameColour))
        {
            return Result.Failure(BoardErrors.RoadAlreadyConnected);
        }

        return Result.Success();
    }

    private bool IsHousePointValid(Point point)
    {
        return buildingManager.GetHouse(point) != null;
    }

    private bool RoadPointsAreValid(Point point1, Point point2)
    {
        return GetRoad(point1, point2) != null;
    }

    private bool TilePointIsValid(Point point)
    {
        return tileManager.GetTile(point) != null;
    }

    private bool RoadIsClaimed(Point point1, Point point2)
    {
        var road = GetRoad(point1, point2);

        if (road is null || road.Colour == PlayerColour.None)
        {
            return false;
        }

        return true;
    }

    private bool RoadAtPointsIsBlockedByOpposingHouse(Point point1, Point point2, PlayerColour colour)
    {
        if ((PointContainsHouseNotOfColour(point1, colour) && GetRoadsOfColourConnectedToPoint(point2, colour).Count == 0)
            || (PointContainsHouseNotOfColour(point2, colour) && GetRoadsOfColourConnectedToPoint(point1, colour).Count == 0))
        {
            return true;
        }

        return false;
    }

    private bool RoadIsConnectedToHouseOrRoadOfSameColour(Point point1, Point point2, PlayerColour colour)
    {
        if (PointContainsHouseOfColour(point1, colour) || PointContainsHouseOfColour(point2, colour))
        {
            return true;
        }

        if (GetRoadsOfColourConnectedToPoint(point1, colour).Count > 0
            || GetRoadsOfColourConnectedToPoint(point2, colour).Count > 0)
        {
            return true;
        }

        return false;
    }

    private bool PointContainsHouseNotOfColour(Point point, PlayerColour colour)
    {
        return buildingManager.GetOccupiedHouseNotOfColour(point, colour) != null;
    }

    private bool PointContainsHouseOfColour(Point point, PlayerColour colour)
    {
        return buildingManager.GetOccupiedHouseOfColour(point, colour) != null;
    }

    private List<Road> GetRoadsOfColourConnectedToPoint(Point point, PlayerColour colour)
    {
        return buildingManager.GetRoadsOfColourAtPoint(point, colour);
    }

    private void UpdateLongestRoadInfo()
    {
        var longestRoadPlayer = PlayerColour.None;
        var longestRoadLength = 0;

        foreach (var colour in Enum.GetValues<PlayerColour>())
        {
            if (colour == PlayerColour.None) continue;

            var lengthOfLongestRoadForColour = GetLengthOfLongestRoadForColour(colour);

            if (lengthOfLongestRoadForColour <= longestRoadLength)
            {
                continue;
            }
            
            longestRoadLength = lengthOfLongestRoadForColour;
            longestRoadPlayer = colour;
        }

        if (longestRoadLength >= 5
            && longestRoadLength > LongestRoadInfo.Length)
        {
            LongestRoadInfo = new LongestRoadInfo(longestRoadPlayer, longestRoadLength);
        }
    }

    private void InitialisePorts()
    {
        var remainingPortTypes = DomainConstants.GetPortTypeTotals();
        var allPortLocations = DomainConstants.GetStartingPortPoints();

        for (var i = 0; i < allPortLocations.Count; i += 2)
        {
            PortType natakPortType;
            int lowestPortTypeNum = (int)remainingPortTypes.First().Key;
            int highestPortTypeNum = (int)remainingPortTypes.Last().Key;

            do
            {
                natakPortType = (PortType)random.Next(lowestPortTypeNum, highestPortTypeNum + 1);
            }
            while (remainingPortTypes[natakPortType] <= 0);

            remainingPortTypes[natakPortType]--;

            var newPort1 = new Port(natakPortType, allPortLocations[i]);
            var newPort2 = new Port(natakPortType, allPortLocations[i + 1]);

            ports.Add(newPort1);
            ports.Add(newPort2);
        }
    }

    private static List<Point> TileToSurroundingHousePoints(Point tilePoint)
    {
        var surroundingHousePoints = new List<Point>();

        int y = tilePoint.Y;

        int firstX = 2 * (tilePoint.X - 1) + y;

        for (int x = firstX; x < firstX + 3; x++)
        {
            for (int j = 0; j < 2; j++)
            {
                surroundingHousePoints.Add(new(x, y + j));
            }
        }

        return surroundingHousePoints;
    }

    private static List<Point> HouseToSurroundingTilePoints(Point housePoint)
    {
        var surroundingTilePoints = new List<Point>();

        int y = housePoint.Y;

        int x = (int)(0.5 * (housePoint.X - housePoint.Y) + 1);

        surroundingTilePoints.Add(new(x, y));
        surroundingTilePoints.Add(new(x, y - 1));

        if ((housePoint.X + housePoint.Y) % 2 != 0)
        {
            surroundingTilePoints.Add(new(x + 1, y - 1));
        }
        else
        {
            surroundingTilePoints.Add(new(x - 1, y));
        }

        return surroundingTilePoints;
    }
}
