using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Domain;

public sealed class Board
{
    private readonly Tile[,] tiles;
    private readonly List<Port> ports = [];
    private readonly Building[,] houses;
    private readonly List<Road> roads = [];

    private readonly Random random = new();

    public Board(int? seed = null)
    {
        if (seed.HasValue)
        {
            random = new(seed.Value);
        }

        tiles = new Tile[BoardLength, BoardLength];
        ports = [];
        houses = new Building[11, 6];
        roads = [];
        RobberPosition = new Point(0, 0);

        InitialiseTilesAndSetRobber();
        InitialiseHouses();
        InitialiseRoads();
        InitialisePorts();
    }

    public Point RobberPosition { get; private set; }

    public int BoardLength { get; private set; } = 5;

    public Tile[,] GetTiles() => tiles;

    public Building[,] GetHouses() => houses;

    public List<Road> GetRoads() => roads;

    public List<Port> GetPorts() => ports;

    public Tile? GetTile(Point point)
    {
        if (TilePointIsValid(point))
        {
            return tiles[point.X, point.Y];
        }

        return null;
    }

    public Building? GetHouse(Point point)
    {
        if (HousePointIsValid(point))
        {
            var house = houses[point.X, point.Y];

            if (house?.Type != BuildingType.Settlement && house?.Type != BuildingType.City)
            {
                return null;
            }

            return houses[point.X, point.Y];
        }

        return null;
    }

    public LongestRoadInfo GetLongestRoadInfo()
    {
        var longestRoadPlayer = PlayerColour.None;
        var longestRoadLength = 0;

        foreach (var colour in Enum.GetValues<PlayerColour>())
        {
            if (colour == PlayerColour.None) continue;

            var lengthOfLongestRoadForColour = GetLengthOfLongestRoadForColour(colour);

            if (lengthOfLongestRoadForColour > longestRoadLength)
            {
                longestRoadLength = lengthOfLongestRoadForColour;
                longestRoadPlayer = colour;
            }
        }

        if (longestRoadLength < 5)
        {
            return new LongestRoadInfo(PlayerColour.None, 0);
        }

        return new LongestRoadInfo(longestRoadPlayer, longestRoadLength);
    }

    public int GetLengthOfLongestRoadForColour(PlayerColour colour)
    {
        if (colour == PlayerColour.None)
        {
            throw new ArgumentException($"{nameof(colour)} must not be {PlayerColour.None}.");
        }

        var roadsOfColour = roads.Where(r => r.Colour == colour).ToList();

        var endRoads = roadsOfColour.Where(r => GetOccupiedRoadsConnectedToPoint(r.FirstPoint).Count == 1
            || GetOccupiedRoadsConnectedToPoint(r.SecondPoint).Count == 1).ToList();

        var endPoints = endRoads.Select(r =>
        {
            if (GetOccupiedRoadsConnectedToPoint(r.FirstPoint).Count == 1)
            {
                return r.FirstPoint;
            }
            else
            {
                return r.SecondPoint;
            }
        }).ToList();

        int furthestDistance = 0;

        foreach (var endPoint in endPoints)
        {
            var distanceFromEnd = GetFurthestDistanceFromPointAlongRoads(0, [], endPoint, roadsOfColour, colour);

            if (distanceFromEnd > furthestDistance)
            {
                furthestDistance = distanceFromEnd;
            }
        }

        return furthestDistance;
    }

    public bool ColourHasPortOfType(PlayerColour colour, PortType portType)
    {
        return ports.Any(p => p.Type == portType && houses[p.Point.X, p.Point.Y].Colour == colour);
    }

    public Result CanMoveRobberToPoint(Point point)
    {
        if (!TilePointIsValid(point) || point.Equals(RobberPosition))
        {
            return Result.Failure(BoardErrors.InvalidTilePoint);
        }

        if (point.Equals(RobberPosition))
        {
            return Result.Failure(BoardErrors.RobberAlreadyAtLocation);
        }

        return Result.Success();
    }

    public Result MoveRobberToPoint(Point point)
    {
        var result = CanMoveRobberToPoint(point);

        if (result.IsFailure)
        {
            return result;
        }

        RobberPosition = point;

        return Result.Success();
    }

    public Result CanUpgradeHouseAtPoint(Point point, PlayerColour colour)
    {
        if (colour == PlayerColour.None)
        {
            return Result.Failure(PlayerErrors.InvalidPlayerColour);
        }

        if (!HousePointIsValid(point))
        {
            return Result.Failure(BoardErrors.InvalidSettlementPoint);
        }

        if (!PointContainsHouseOfColour(point, colour))
        {
            return Result.Failure(BoardErrors.SettlementNotOwnedByPlayer);
        }

        if (houses[point.X, point.Y].Type != BuildingType.Settlement)
        {
            return Result.Failure(BoardErrors.SettlementAlreadyUpgraded);
        }

        return Result.Success();
    }

    public Result UpgradeHouse(Point point, PlayerColour colour)
    {
        var canUpgradeResult = CanUpgradeHouseAtPoint(point, colour);

        if (canUpgradeResult.IsFailure)
        {
            return canUpgradeResult;
        }

        houses[point.X, point.Y].SetTypeToCity();

        return Result.Success();
    }

    public Result CanPlaceHouseAtPoint(Point point, PlayerColour colour, bool isFirstTurn = false)
    {
        if (colour == PlayerColour.None)
        {
            return Result.Failure(PlayerErrors.InvalidPlayerColour);
        }

        if (!HousePointIsValid(point))
        {
            return Result.Failure(BoardErrors.InvalidSettlementPoint);
        }

        if (PointContainsHouse(point))
        {
            return Result.Failure(BoardErrors.SettlementAlreadyExists);
        }

        if (HousePointIsTooCloseToAnotherHouse(point))
        {
            return Result.Failure(BoardErrors.SettlementIsTooClose);
        }

        if (!isFirstTurn && GetOccupiedRoadsOfColourConnectedToPoint(point, colour).Count == 0)
        {
            return Result.Failure(BoardErrors.SettlementDoesNotConnect);
        }

        return Result.Success();
    }

    public Result PlaceHouse(Point point, PlayerColour colour, bool isFirstTurn = false)
    {
        var canPlaceResult = CanPlaceHouseAtPoint(point, colour, isFirstTurn);

        if (canPlaceResult.IsFailure)
        {
            return canPlaceResult;
        }

        houses[point.X, point.Y].SetColour(colour);
        houses[point.X, point.Y].SetTypeToHouse();

        return Result.Success();
    }

    public Result CanPlaceSetupRoadBetweenPoints(Point point1, Point point2, PlayerColour colour)
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
        if ((GetOccupiedRoadsOfColourConnectedToPoint(point1, colour).Count > 0
            && point1ContainsHouseOfSameColour)
            || (GetOccupiedRoadsOfColourConnectedToPoint(point2, colour).Count > 0
            && point2ContainsHouseOfSameColour))
        {
            return Result.Failure(BoardErrors.RoadAlreadyConnected);
        }

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

    public Result CanPlaceTwoRoadsBetweenPoints(
        Point point1,
        Point point2,
        Point point3,
        Point point4,
        PlayerColour colour)
    {
        var canPlaceFirstResult = CanPlaceRoadBetweenPoints(point1, point2, colour);

        if (canPlaceFirstResult.IsFailure)
        {
            return canPlaceFirstResult;
        }

        var placeFirstResult = PlaceRoad(point1, point2, colour);

        if (placeFirstResult.IsFailure)
        {
            throw new Exception("Cannot place road.");
        }

        var canPlaceSecondResult = CanPlaceRoadBetweenPoints(point3, point4, colour);

        var removeFirstResult = RemoveRoad(point1, point2);

        if (removeFirstResult.IsFailure)
        {
            throw new Exception("Cannot remove road.");
        }

        return canPlaceSecondResult;
    }

    public Result PlaceRoad(Point point1, Point point2, PlayerColour colour, bool isSetup = false)
    {
        Result canPlaceResult;

        if (isSetup)
        {
            canPlaceResult = CanPlaceSetupRoadBetweenPoints(point1, point2, colour);
        }
        else
        {
            canPlaceResult = CanPlaceRoadBetweenPoints(point1, point2, colour);
        }

        if (canPlaceResult.IsFailure)
        {
            return canPlaceResult;
        }

        var roadInList = GetRoadAtPoints(point1, point2) ?? throw new Exception("Road not found.");

        roadInList.SetColour(colour);

        return Result.Success();
    }

    public List<Point> GetPointsOfTilesWithActivationNumber(int activationNumber)
    {
        if (activationNumber < 2 || activationNumber > 12)
        {
            throw new ArgumentException("Activation number must be between 2 and 12.");
        }

        var tilesWithActivationNumber = new List<Point>();

        for (int x = 0; x < BoardLength; x++)
        {
            for (int y = 0; y < BoardLength; y++)
            {
                if (tiles[x, y] != null && tiles[x, y].ActivationNumber == activationNumber)
                {
                    tilesWithActivationNumber.Add(new(x, y));
                }
            }
        }

        return tilesWithActivationNumber;
    }

    public List<PlayerColour> GetHouseColoursOnTile(Point point)
    {
        if (!TilePointIsValid(point))
        {
            throw new ArgumentException("Point are not valid.");
        }

        var surroundingHouses = GetHousesOnTile(point);

        var houseColours = new List<PlayerColour>();

        foreach (var house in surroundingHouses)
        {
            houseColours.Add(house.Colour);
        }

        return houseColours;
    }

    public List<Building> GetHousesOnTile(Point point)
    {
        if (!TilePointIsValid(point))
        {
            throw new ArgumentException("Point are not valid.");
        }

        var surroundingHousePoints = TileToSurroundingHousePoints(point);

        var housesOnTile = new List<Building>();

        foreach (var housePoint in surroundingHousePoints)
        {
            if (!HousePointIsValid(housePoint))
            {
                continue;
            }

            var house = houses[housePoint.X, housePoint.Y];

            if (house != null && house.Type != BuildingType.None)
            {
                housesOnTile.Add(house);
            }
        }

        return housesOnTile;
    }

    public List<Tile> GetTilesSurroundingHouse(Point point)
    {
        if (!HousePointIsValid(point))
        {
            throw new ArgumentException("Point are not valid.");
        }

        var surroundingTilePoints = HouseToSurroundingTilePoints(point);

        var tilesSurroundingHouse = new List<Tile>();

        foreach (var tilePoint in surroundingTilePoints)
        {
            if (!TilePointIsValid(tilePoint))
            {
                continue;
            }

            var tile = tiles[tilePoint.X, tilePoint.Y];

            if (tile != null)
            {
                tilesSurroundingHouse.Add(tile);
            }
        }

        return tilesSurroundingHouse;
    }

    private bool HousePointIsTooCloseToAnotherHouse(Point point)
    {
        var roadsConnectedToPoint = GetRoadsPositionsConnectedToPoint(point);

        foreach (var road in roadsConnectedToPoint)
        {
            if (PointContainsHouse(road.FirstPoint) || PointContainsHouse(road.SecondPoint))
            {
                return true;
            }
        }

        return false;
    }

    private bool HousePointIsValid(Point point)
    {
        if (point.X < 0 || point.Y < 0 || point.X >= houses.GetLength(0) || point.Y >= houses.GetLength(1))
        {
            return false;
        }

        if (houses[point.X, point.Y] is null)
        {
            return false;
        }

        return true;
    }

    private bool RoadPointsAreValid(Point point1, Point point2)
    {
        if (GetRoadAtPoints(point1, point2) is null)
        {
            return false;
        }

        return true;
    }

    private Result RemoveRoad(Point point1, Point point2)
    {
        var road = GetRoadAtPoints(point1, point2);

        if (road is null)
        {
            return Result.Failure(BoardErrors.RoadNotFound);
        }

        road.SetColour(PlayerColour.None);

        return Result.Success();
    }

    private bool TilePointIsValid(Point point)
    {
        if (point.X < 0 || point.Y < 0 || point.X >= tiles.GetLength(0) || point.Y >= tiles.GetLength(1))
        {
            return false;
        }

        if (tiles[point.X, point.Y] is null)
        {
            return false;
        }

        return true;
    }

    private bool RoadIsClaimed(Point point1, Point point2)
    {
        var road = GetRoadAtPoints(point1, point2);

        if (road is null || road.Colour == PlayerColour.None)
        {
            return false;
        }

        return true;
    }

    private bool RoadAtPointsIsBlockedByOpposingHouse(Point point1, Point point2, PlayerColour colour)
    {
        if ((PointContainsHouseNotOfColour(point1, colour) && GetOccupiedRoadsOfColourConnectedToPoint(point2, colour).Count == 0)
            || (PointContainsHouseNotOfColour(point2, colour) && GetOccupiedRoadsOfColourConnectedToPoint(point1, colour).Count == 0))
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

        if (GetOccupiedRoadsOfColourConnectedToPoint(point1, colour).Count > 0
            || GetOccupiedRoadsOfColourConnectedToPoint(point2, colour).Count > 0)
        {
            return true;
        }

        return false;
    }

    private bool PointContainsHouseNotOfColour(Point point, PlayerColour colour)
    {
        var house = houses[point.X, point.Y];
        return house?.Colour != colour && house?.Type != BuildingType.None;
    }

    private bool PointContainsHouseOfColour(Point point, PlayerColour colour)
    {
        var house = houses[point.X, point.Y];
        return house?.Colour == colour && house?.Type != BuildingType.None;
    }

    private bool PointContainsHouse(Point point)
    {
        return houses[point.X, point.Y]?.Type != BuildingType.None;
    }

    private Road? GetRoadAtPoints(Point point1, Point point2)
    {
        var roadInList = roads.FirstOrDefault(r => r.FirstPoint.Equals(point1) && r.SecondPoint.Equals(point2));

        roadInList ??= roads.FirstOrDefault(r => r.FirstPoint.Equals(point2) && r.SecondPoint.Equals(point1));

        return roadInList;
    }

    private List<Road> GetOccupiedRoadsOfColourConnectedToPoint(Point point, PlayerColour colour)
    {
        var roadsConnectedToPoint = GetRoadsPositionsConnectedToPoint(point);
        var occupiedRoadsConnectedToPoint = new List<Road>();

        foreach (var road in roadsConnectedToPoint)
        {
            if (road.Colour == colour)
            {
                occupiedRoadsConnectedToPoint.Add(road);
            }
        }

        return occupiedRoadsConnectedToPoint;
    }

    private List<Road> GetOccupiedRoadsConnectedToPoint(Point point)
    {
        var roadsConnectedToPoint = GetRoadsPositionsConnectedToPoint(point);
        var occupiedRoadsConnectedToPoint = new List<Road>();

        foreach (var road in roadsConnectedToPoint)
        {
            if (road.Colour != PlayerColour.None)
            {
                occupiedRoadsConnectedToPoint.Add(road);
            }
        }

        return occupiedRoadsConnectedToPoint;
    }

    private List<Road> GetRoadsPositionsConnectedToPoint(Point point)
    {
        var roadsConnectedToPoint = new List<Road>();

        foreach (var road in roads)
        {
            if (road.FirstPoint.Equals(point) || road.SecondPoint.Equals(point))
            {
                roadsConnectedToPoint.Add(road);
            }
        }

        return roadsConnectedToPoint;
    }

    private int GetFurthestDistanceFromPointAlongRoads(
        int distanceTravelled,
        List<Road> checkedRoads,
        Point point,
        List<Road> roads,
        PlayerColour colour)
    {
        if (colour == PlayerColour.None)
        {
            throw new ArgumentException($"{nameof(colour)} must not be {PlayerColour.None}.");
        }

        var connectedRoads = GetOccupiedRoadsConnectedToPoint(point);

        var connectedRoadsNotChecked = connectedRoads.Where(r => !checkedRoads.Contains(r)).ToList();

        if (connectedRoadsNotChecked.Count == 0
        || PointContainsHouseNotOfColour(point, colour))
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

            var distanceAlongRoad = GetFurthestDistanceFromPointAlongRoads(distanceTravelled + 1, checkedRoads, newPoint, roads, colour);

            if (distanceAlongRoad > furthestDistanceFromPoint)
            {
                furthestDistanceFromPoint = distanceAlongRoad;
            }
        }

        return furthestDistanceFromPoint;
    }

    private void InitialiseTilesAndSetRobber()
    {
        var remainingResourceTileTypes = DomainConstants.GetTileResourceTypeTotals();
        var remainingActivationNumbers = DomainConstants.GetTileActivationNumberTotals();

        for (int x = 0; x < BoardLength; x++)
        {
            for (int y = 0; y < BoardLength; y++)
            {
                if (x + y >= 2 && x + y <= BoardLength + 1)
                {
                    tiles[x, y] = CreateNewCatanTile(remainingResourceTileTypes, remainingActivationNumbers);

                    if (tiles[x, y].Type == ResourceType.Desert)
                    {
                        RobberPosition = new Point(x, y);
                    }
                }
            }
        }
    }

    private void InitialiseHouses()
    {
        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                if (x + y >= 2 && x + y <= 13 && y - x <= 3 && y - x >= -8)
                {
                    houses[x, y] = new Building(PlayerColour.None, BuildingType.None);
                }
            }
        }
    }

    private void InitialiseRoads()
    {
        List<Point> roadVectors =
        [
            new Point(0, -1),
            new Point(-1, 0)
        ];

        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                var house = houses[x, y];

                if (house is null) continue;

                var roadCorner1 = new Point(x, y);

                foreach (var vector in roadVectors)
                {
                    var roadCorner2 = Point.Add(vector, roadCorner1);

                    if (roadCorner2.X < 0 || roadCorner2.Y < 0) continue;

                    var road = new Road(PlayerColour.None, roadCorner1, roadCorner2);

                    if (houses[roadCorner2.X, roadCorner2.Y] is null) continue;

                    if (Math.Min(roadCorner1.Y, roadCorner2.Y) % 2 == roadCorner1.X % 2 || vector.Y == 0)
                    {
                        roads.Add(road);
                    }
                }
            }
        }
    }

    private void InitialisePorts()
    {
        var remainingPortTypes = DomainConstants.GetPortTypeTotals();
        var allPortLocations = DomainConstants.GetStartingPortPoints();

        for (var i = 0; i < allPortLocations.Count; i += 2)
        {
            PortType catanPortType;
            int lowestPortTypeNum = (int)remainingPortTypes.First().Key;
            int highestPortTypeNum = (int)remainingPortTypes.Last().Key;

            do
            {
                catanPortType = (PortType)random.Next(lowestPortTypeNum, highestPortTypeNum + 1);
            }
            while (remainingPortTypes[catanPortType] <= 0);

            remainingPortTypes[catanPortType]--;

            var newPort1 = new Port(catanPortType, allPortLocations[i]);
            var newPort2 = new Port(catanPortType, allPortLocations[i + 1]);

            ports.Add(newPort1);
            ports.Add(newPort2);
        }
    }

    private Tile CreateNewCatanTile(Dictionary<ResourceType, int> remainingResourceTileTypes, Dictionary<int, int> remainingActivationNumbers)
    {
        if (remainingResourceTileTypes is null || remainingResourceTileTypes.Count == 0)
        {
            throw new ArgumentException($"{nameof(remainingResourceTileTypes)} must not be null or empty.");
        }

        if (remainingActivationNumbers is null || remainingActivationNumbers.Count == 0)
        {
            throw new ArgumentException($"{nameof(remainingActivationNumbers)} must not be null or empty.");
        }

        ResourceType catanTileType;
        int lowestTileTypeNum = (int)remainingResourceTileTypes.First().Key;
        int highestTileTypeNum = (int)remainingResourceTileTypes.Last().Key;

        do
        {
            catanTileType = (ResourceType)random.Next(lowestTileTypeNum, highestTileTypeNum + 1);
        }
        while (remainingResourceTileTypes[catanTileType] <= 0);

        remainingResourceTileTypes[catanTileType]--;

        if (catanTileType == ResourceType.Desert)
        {
            return new Tile(catanTileType, 0);
        }

        int activationNumber;
        int lowestActivationNum = remainingActivationNumbers.First().Key;
        int highestActivationNum = remainingActivationNumbers.Last().Key;

        do
        {
            activationNumber = random.Next(lowestActivationNum, highestActivationNum + 1);
        }
        while (remainingActivationNumbers[activationNumber] <= 0);

        remainingActivationNumbers[activationNumber]--;

        return new Tile(catanTileType, activationNumber);
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
