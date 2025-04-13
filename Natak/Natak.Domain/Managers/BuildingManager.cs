using Natak.Domain.Enums;
using Natak.Domain.Errors;

namespace Natak.Domain.Managers;

public sealed class BuildingManager
{
    private const int XLimit = 11;
    private const int YLimit = 6;
    
    private readonly List<House> houses = [];
    private readonly List<Road> roads = [];
    
    public BuildingManager(
        List<House> houses,
        List<Road> roads)
    {
        this.houses = houses;
        this.roads = roads;
    }
    
    public BuildingManager()
    {
        GenerateHouses();
        GenerateRoads();
    }
    
    public List<House> GetHouses()
        => houses;
    
    public List<Road> GetRoads()
        => roads;
    
    public List<Road> GetRoads(PlayerColour colour)
        => roads.Where(x => x.Colour == colour).ToList();
    
    public List<Road> GetRoadsOfColourAtPoint(Point point, PlayerColour colour)
        => GetRoadsAtPoint(point)
            .Where(x => x.Colour == colour)
            .ToList();
    
    public List<Road> GetOccupiedRoadsAtPoint(Point point)
        => GetRoadsAtPoint(point)
            .Where(x => x.Colour != PlayerColour.None)
            .ToList();
    
    public House? GetHouse(Point point)
        => houses.FirstOrDefault(x => x.Point.Equals(point));
    
    public House? GetOccupiedHouse(Point point)
    { 
        var house = GetHouse(point);
        
        if (house is null)
        {
            return null;
        }

        return house.Type != HouseType.None
            ? house
            : null;
    }
    
    public House? GetOccupiedHouseOfColour(Point point, PlayerColour colour)
    {
        var house = GetOccupiedHouse(point);

        return house?.Colour == colour
            ? house
            : null;
    }
    
    public House? GetOccupiedHouseNotOfColour(Point point, PlayerColour colour)
    {
        var house = GetOccupiedHouse(point);
        
        if (house is null)
        {
            return null;
        }

        return house.Colour != colour
            ? house
            : null;
    }

    public List<Point> GetUnoccupiedHousePoints()
    {
        return houses
            .Where(x => x.Type == HouseType.None)
            .Select(x => x.Point)
            .ToList();
    }
    
    public Road? GetRoad(Point firstPoint, Point secondPoint)
        => roads.FirstOrDefault(x => x.IsAtPoints(firstPoint, secondPoint));
    
    public Result CanAddVillage(House house)
    {
        if (house.Colour == PlayerColour.None)
        {
            return Result.Failure(PlayerErrors.InvalidPlayerColour);
        }
        
        if (IsHousePointTooCloseToAnotherHouse(house.Point))
        {
            return Result.Failure(BoardErrors.VillageIsTooClose);
        }
        
        var existingHouse = GetHouse(house.Point);

        if (existingHouse is null)
        {
            return Result.Failure(BoardErrors.InvalidVillagePoint);
        }

        return existingHouse.Colour == PlayerColour.None
            ? Result.Success()
            : Result.Failure(BoardErrors.VillageAlreadyExists);
    }
    
    public Result AddVillage(House house)
    {
        var existingHouse = GetHouse(house.Point);
        
        if (existingHouse is null)
        {
            throw new InvalidOperationException("Village not found.");
        }
        
        existingHouse.SetColour(house.Colour);
        existingHouse.SetTypeToVillage();
        
        return Result.Success();
    }
    
    public Result CanUpgradeVillageAtPoint(Point point)
    {
        var village = GetHouse(point);
        
        if (village is null)
        {
            return Result.Failure(BoardErrors.InvalidVillagePoint);
        }
        
        if (village.Type == HouseType.None)
        {
            return Result.Failure(BoardErrors.VillageNotFound);
        }
        
        if (village.Colour == PlayerColour.None)
        {
            return Result.Failure(PlayerErrors.InvalidPlayerColour);
        }
        
        return village.Type == HouseType.Town
            ? Result.Failure(BoardErrors.VillageAlreadyUpgraded)
            : Result.Success();
    }
    
    public Result UpgradeVillageAtPoint(Point point)
    {
        var village = GetHouse(point);
        
        if (village is null)
        {
            throw new InvalidOperationException("Village not found.");
        }
        
        village.SetTypeToTown();
        
        return Result.Success();
    }
    
    public Result CanAddRoad(Road road)
    {
        if (road.Colour == PlayerColour.None)
        {
            return Result.Failure(PlayerErrors.InvalidPlayerColour);
        }
        
        var existingRoad = GetRoad(road.FirstPoint, road.SecondPoint);
        
        if (existingRoad is null)
        {
            return Result.Failure(BoardErrors.InvalidRoadPoints);
        }

        return existingRoad.Colour == PlayerColour.None
            ? Result.Success()
            : Result.Failure(BoardErrors.RoadAlreadyExists);
    }
    
    public Result AddRoad(Road road)
    {
        var existingRoad = GetRoad(road.FirstPoint, road.SecondPoint);
        
        if (existingRoad is null)
        {
            throw new InvalidOperationException("Road not found.");
        }
        
        existingRoad.SetColour(road.Colour);
        
        return Result.Success();
    }
    
    public int GetLengthOfLongestRoadForColour(PlayerColour colour)
    {
        if (colour == PlayerColour.None)
        {
            throw new ArgumentException($"{nameof(colour)} must not be {PlayerColour.None}.");
        }

        var roadsOfColour = GetRoads(colour);

        var endRoads = roadsOfColour.Where(r => GetRoadsOfColourAtPoint(r.FirstPoint, colour).Count == 1
                                                || GetRoadsOfColourAtPoint(r.SecondPoint, colour).Count == 1).ToList();

        var endPoints = endRoads
            .Select(r =>
                GetRoadsOfColourAtPoint(r.FirstPoint, colour).Count == 1
                    ? r.FirstPoint
                    : r.SecondPoint)
            .ToList();

        var furthestDistance = 0;

        foreach (var endPoint in endPoints)
        {
            var distanceFromEnd = GetFurthestDistanceFromPointAlongRoads(0, [], endPoint, colour);

            if (distanceFromEnd > furthestDistance)
            {
                furthestDistance = distanceFromEnd;
            }
        }

        return furthestDistance;
    }
    
    private bool IsHousePointTooCloseToAnotherHouse(Point point)
    {
        var roadsConnectedToPoint = GetRoadsAtPoint(point);

        return roadsConnectedToPoint.Any(
            road => GetHouse(road.FirstPoint)?.Type != HouseType.None
                    || GetHouse(road.SecondPoint)?.Type != HouseType.None);
    }

    private void GenerateHouses()
    {
        for (var x = 0; x < XLimit; x++)
        {
            for (var y = 0; y < YLimit; y++)
            {
                var point = new Point(x, y);
                
                if (IsValidHousePoint(point))
                {
                    houses.Add(new House(point));
                }
            }
        }
    }
    
    private static bool IsValidHousePoint(Point point)
    {
        var x = point.X;
        var y = point.Y;

        return x + y >= 2 && x + y <= 13 && y - x <= 3 && y - x >= -8;
    }
    
    private void GenerateRoads()
    {
        List<Point> roadVectors =
        [
            new(0, -1),
            new(-1, 0)
        ];

        for (var x = 0; x < XLimit; x++)
        {
            for (var y = 0; y < YLimit; y++)
            {
                var point = new Point(x, y);
                
                var house = GetHouse(point);

                if (house is null) continue;

                var roadCorner1 = new Point(x, y);

                foreach (var vector in roadVectors)
                {
                    var roadCorner2 = Point.Add(vector, roadCorner1);

                    if (roadCorner2.X < 0 || roadCorner2.Y < 0) continue;

                    var road = new Road(roadCorner1, roadCorner2);

                    if (GetHouse(roadCorner2) is null) continue;

                    if (Math.Min(roadCorner1.Y, roadCorner2.Y) % 2 == roadCorner1.X % 2 || vector.Y == 0)
                    {
                        roads.Add(road);
                    }
                }
            }
        }
    }

    private List<Road> GetRoadsAtPoint(Point point)
    {
        return roads
            .Where(x => x.FirstPoint.Equals(point)
                        || x.SecondPoint.Equals(point))
            .ToList();
    }
    
    private int GetFurthestDistanceFromPointAlongRoads(
        int distanceTravelled,
        List<Road> checkedRoads,
        Point point,
        PlayerColour colour)
    {
        if (colour == PlayerColour.None)
        {
            throw new ArgumentException($"{nameof(colour)} must not be {PlayerColour.None}.");
        }

        var connectedRoads = GetRoadsOfColourAtPoint(point, colour);

        var connectedRoadsNotChecked = connectedRoads.Where(r => !checkedRoads.Contains(r)).ToList();

        if (connectedRoadsNotChecked.Count == 0
            || GetOccupiedHouseNotOfColour(point, colour) != null)
        {
            return distanceTravelled;
        }

        var furthestDistanceFromPoint = 0;

        foreach (var road in connectedRoadsNotChecked)
        {
            checkedRoads.Add(road);

            var newPoint = road.FirstPoint.Equals(point)
                ? road.SecondPoint
                : road.FirstPoint;

            var distanceAlongRoad = GetFurthestDistanceFromPointAlongRoads(distanceTravelled + 1, checkedRoads, newPoint, colour);

            if (distanceAlongRoad > furthestDistanceFromPoint)
            {
                furthestDistanceFromPoint = distanceAlongRoad;
            }
        }

        return furthestDistanceFromPoint;
    }
}