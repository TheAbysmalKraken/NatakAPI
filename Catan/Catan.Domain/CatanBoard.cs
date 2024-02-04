using static Catan.Common.Enumerations;

namespace Catan.Domain;

public sealed class CatanBoard
{
    private readonly CatanTile[,] tiles;
    private readonly List<CatanPort> ports = new();
    private readonly CatanBuilding[,] houses;
    private readonly List<CatanRoad> roads = new();

    private readonly Random random = new();

    public CatanBoard()
    {
        tiles = new CatanTile[BoardLength, BoardLength];
        ports = new List<CatanPort>();
        houses = new CatanBuilding[11, 6];
        roads = new List<CatanRoad>();
        RobberPosition = new Coordinates(0, 0);

        InitialiseTilesAndSetRobber();
        InitialiseHouses();
        InitialiseRoads();
        InitialisePorts();
    }

    public Coordinates RobberPosition { get; private set; }

    public int BoardLength { get; private set; } = 5;

    public CatanTile[,] GetTiles() => tiles;

    public CatanTile GetTile(int x, int y) => tiles[x, y];

    public CatanBuilding[,] GetHouses() => houses;

    public CatanBuilding GetHouse(int x, int y) => houses[x, y];

    public List<CatanRoad> GetRoads() => roads;

    public List<CatanPort> GetPorts() => ports;

    public bool CanUpgradeHouseAtCoordinates(Coordinates coordinates, CatanPlayerColour colour)
    {
        if (colour == CatanPlayerColour.None
            || !HouseCoordinatesAreValid(coordinates)
            || !PointContainsHouseOfColour(coordinates, colour)
            || houses[coordinates.X, coordinates.Y].Type != CatanBuildingType.Settlement)
        {
            return false;
        }

        return true;
    }

    public void UpgradeHouse(Coordinates coordinates, CatanPlayerColour colour)
    {
        if (!CanUpgradeHouseAtCoordinates(coordinates, colour))
        {
            throw new ArgumentException("Cannot upgrade house at these coordinates.");
        }

        houses[coordinates.X, coordinates.Y].SetTypeToCity();
    }

    public bool CanPlaceHouseAtCoordinates(Coordinates coordinates, CatanPlayerColour colour)
    {
        if (colour == CatanPlayerColour.None
            || !HouseCoordinatesAreValid(coordinates)
            || PointContainsHouse(coordinates)
            || HouseCoordinatesAreTooCloseToAnotherHouse(coordinates))
        {
            return false;
        }

        return true;
    }

    public void PlaceHouse(Coordinates coordinates, CatanPlayerColour colour)
    {
        if (!CanPlaceHouseAtCoordinates(coordinates, colour))
        {
            throw new ArgumentException("Cannot place house at these coordinates.");
        }

        houses[coordinates.X, coordinates.Y].SetColour(colour);
        houses[coordinates.X, coordinates.Y].SetTypeToHouse();
    }

    public bool CanPlaceRoadBetweenCoordinates(Coordinates coordinates1, Coordinates coordinates2, CatanPlayerColour colour)
    {
        if (colour == CatanPlayerColour.None
            || !RoadCoordinatesAreValid(coordinates1, coordinates2)
            || RoadIsClaimed(coordinates1, coordinates2)
            || RoadAtCoordinatesIsBlockedByOpposingHouse(coordinates1, coordinates2, colour)
            || !RoadIsConnectedToHouseOrRoadOfSameColour(coordinates1, coordinates2, colour))
        {
            return false;
        }

        return true;
    }

    public void PlaceRoad(Coordinates coordinates1, Coordinates coordinates2, CatanPlayerColour colour)
    {
        if (!CanPlaceRoadBetweenCoordinates(coordinates1, coordinates2, colour))
        {
            throw new ArgumentException("Cannot place road between these coordinates.");
        }

        var roadInList = GetRoadAtCoordinates(coordinates1, coordinates2);

        if (roadInList is null)
        {
            throw new ArgumentException("Cannot find road in list.");
        }

        roadInList.SetColour(colour);
    }

    private bool HouseCoordinatesAreTooCloseToAnotherHouse(Coordinates coordinates)
    {
        var roadsConnectedToPoint = GetRoadsPositionsConnectedToPoint(coordinates);

        foreach (var road in roadsConnectedToPoint)
        {
            if (PointContainsHouse(road.FirstCornerCoordinates) || PointContainsHouse(road.SecondCornerCoordinates))
            {
                return true;
            }
        }

        return false;
    }

    private bool HouseCoordinatesAreValid(Coordinates coordinates)
    {
        if (coordinates.X < 0 || coordinates.Y < 0 || coordinates.X >= houses.GetLength(0) || coordinates.Y >= houses.GetLength(1))
        {
            return false;
        }

        if (houses[coordinates.X, coordinates.Y] is null)
        {
            return false;
        }

        return true;
    }

    private bool RoadCoordinatesAreValid(Coordinates coordinates1, Coordinates coordinates2)
    {
        if (GetRoadAtCoordinates(coordinates1, coordinates2) is null)
        {
            return false;
        }

        return true;
    }

    private bool RoadIsClaimed(Coordinates coordinates1, Coordinates coordinates2)
    {
        var road = GetRoadAtCoordinates(coordinates1, coordinates2);

        if (road is null || road.Colour == CatanPlayerColour.None)
        {
            return false;
        }

        return true;
    }

    private bool RoadAtCoordinatesIsBlockedByOpposingHouse(Coordinates coordinates1, Coordinates coordinates2, CatanPlayerColour colour)
    {
        if ((PointContainsHouseNotOfColour(coordinates1, colour) && GetOccupiedRoadsOfColourConnectedToPoint(coordinates2, colour).Count == 0)
            || (PointContainsHouseNotOfColour(coordinates2, colour) && GetOccupiedRoadsOfColourConnectedToPoint(coordinates1, colour).Count == 0))
        {
            return true;
        }

        return false;
    }

    private bool RoadIsConnectedToHouseOrRoadOfSameColour(Coordinates coordinates1, Coordinates coordinates2, CatanPlayerColour colour)
    {
        if (PointContainsHouseOfColour(coordinates1, colour) || PointContainsHouseOfColour(coordinates2, colour))
        {
            return true;
        }

        if (GetOccupiedRoadsOfColourConnectedToPoint(coordinates1, colour).Count > 0
            || GetOccupiedRoadsOfColourConnectedToPoint(coordinates2, colour).Count > 0)
        {
            return true;
        }

        return false;
    }

    private bool PointContainsHouseNotOfColour(Coordinates coordinates, CatanPlayerColour colour)
    {
        var house = houses[coordinates.X, coordinates.Y];
        return house?.Colour != colour && house?.Type != CatanBuildingType.None;
    }

    private bool PointContainsHouseOfColour(Coordinates coordinates, CatanPlayerColour colour)
    {
        var house = houses[coordinates.X, coordinates.Y];
        return house?.Colour == colour && house?.Type != CatanBuildingType.None;
    }

    private bool PointContainsHouse(Coordinates coordinates)
    {
        return houses[coordinates.X, coordinates.Y]?.Type != CatanBuildingType.None;
    }

    private CatanRoad? GetRoadAtCoordinates(Coordinates coordinates1, Coordinates coordinates2)
    {
        var roadInList = roads.FirstOrDefault(r => r.FirstCornerCoordinates.Equals(coordinates1) && r.SecondCornerCoordinates.Equals(coordinates2));

        if (roadInList is null)
        {
            roadInList = roads.FirstOrDefault(r => r.FirstCornerCoordinates.Equals(coordinates2) && r.SecondCornerCoordinates.Equals(coordinates1));
        }

        return roadInList;
    }

    private List<CatanRoad> GetOccupiedRoadsOfColourConnectedToPoint(Coordinates coordinates, CatanPlayerColour colour)
    {
        var roadsConnectedToPoint = GetRoadsPositionsConnectedToPoint(coordinates);
        var occupiedRoadsConnectedToPoint = new List<CatanRoad>();

        foreach (var road in roadsConnectedToPoint)
        {
            if (road.Colour == colour)
            {
                occupiedRoadsConnectedToPoint.Add(road);
            }
        }

        return occupiedRoadsConnectedToPoint;
    }

    private List<CatanRoad> GetOccupiedRoadsConnectedToPoint(Coordinates coordinates)
    {
        var roadsConnectedToPoint = GetRoadsPositionsConnectedToPoint(coordinates);
        var occupiedRoadsConnectedToPoint = new List<CatanRoad>();

        foreach (var road in roadsConnectedToPoint)
        {
            if (road.Colour != CatanPlayerColour.None)
            {
                occupiedRoadsConnectedToPoint.Add(road);
            }
        }

        return occupiedRoadsConnectedToPoint;
    }

    private List<CatanRoad> GetRoadsPositionsConnectedToPoint(Coordinates coordinates)
    {
        var roadsConnectedToPoint = new List<CatanRoad>();

        foreach (var road in roads)
        {
            if (road.FirstCornerCoordinates.Equals(coordinates) || road.SecondCornerCoordinates.Equals(coordinates))
            {
                roadsConnectedToPoint.Add(road);
            }
        }

        return roadsConnectedToPoint;
    }

    private void InitialiseTilesAndSetRobber()
    {
        var remainingResourceTileTypes = DomainConstants.GetTileResourceTypeTotals();
        var remainingActivationNumbers = DomainConstants.GetTileActivationNumberTotals();

        for (int i = 0; i < BoardLength; i++)
        {
            for (int j = 0; j < BoardLength; j++)
            {
                if (i + j >= 2 && i + j <= BoardLength + 1)
                {
                    tiles[i, j] = CreateNewCatanTile(remainingResourceTileTypes, remainingActivationNumbers);

                    if (tiles[i, j].Type == CatanResourceType.Desert)
                    {
                        RobberPosition = new Coordinates(i, j);
                    }
                }
            }
        }
    }

    private void InitialiseHouses()
    {
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (i + j >= 2 && i + j <= 13 && j - i <= 3 && j - i >= -8)
                {
                    houses[i, j] = new CatanBuilding(CatanPlayerColour.None, CatanBuildingType.None);
                }
            }
        }
    }

    private void InitialiseRoads()
    {
        List<Coordinates> roadVectors = new()
        {
            new Coordinates(0, -1),
            new Coordinates(-1, 0)
        };

        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                var house = houses[i, j];

                if (house is null) continue;

                var roadCorner1 = new Coordinates(i, j);

                foreach (var vector in roadVectors)
                {
                    var roadCorner2 = Coordinates.Add(vector, roadCorner1);

                    if (roadCorner2.X < 0 || roadCorner2.Y < 0) continue;

                    var road = new CatanRoad(CatanPlayerColour.None, roadCorner1, roadCorner2);

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
        var allPortLocations = DomainConstants.GetStartingPortCoordinates();

        for (var i = 0; i < allPortLocations.Count; i += 2)
        {
            CatanPortType catanPortType;
            int lowestPortTypeNum = (int)remainingPortTypes.First().Key;
            int highestPortTypeNum = (int)remainingPortTypes.Last().Key;

            do
            {
                catanPortType = (CatanPortType)random.Next(lowestPortTypeNum, highestPortTypeNum + 1);
            }
            while (remainingPortTypes[catanPortType] <= 0);

            remainingPortTypes[catanPortType]--;

            var newPort1 = new CatanPort(catanPortType, allPortLocations[i]);
            var newPort2 = new CatanPort(catanPortType, allPortLocations[i + 1]);

            ports.Add(newPort1);
            ports.Add(newPort2);
        }
    }

    private CatanTile CreateNewCatanTile(Dictionary<CatanResourceType, int> remainingResourceTileTypes, Dictionary<int, int> remainingActivationNumbers)
    {
        if (remainingResourceTileTypes is null || remainingResourceTileTypes.Count == 0)
        {
            throw new ArgumentException($"{nameof(remainingResourceTileTypes)} must not be null or empty.");
        }

        if (remainingActivationNumbers is null || remainingActivationNumbers.Count == 0)
        {
            throw new ArgumentException($"{nameof(remainingActivationNumbers)} must not be null or empty.");
        }

        CatanResourceType catanTileType;
        int lowestTileTypeNum = (int)remainingResourceTileTypes.First().Key;
        int highestTileTypeNum = (int)remainingResourceTileTypes.Last().Key;

        do
        {
            catanTileType = (CatanResourceType)random.Next(lowestTileTypeNum, highestTileTypeNum + 1);
        }
        while (remainingResourceTileTypes[catanTileType] <= 0);

        remainingResourceTileTypes[catanTileType]--;

        if (catanTileType == CatanResourceType.Desert)
        {
            return new CatanTile(catanTileType, 0);
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

        return new CatanTile(catanTileType, activationNumber);
    }
}
