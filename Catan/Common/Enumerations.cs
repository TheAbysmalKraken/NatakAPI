namespace Catan.Common;

public static class Enumerations
{
    public enum CatanResourceType
    {
        None = 0,
        Wood = 1,
        Brick = 2,
        Sheep = 3,
        Wheat = 4,
        Ore = 5,
        Desert = 6
    }

    public enum CatanPlayerColour
    {
        None = -1,
        Red = 0,
        Blue = 1,
        Green = 2,
        Yellow = 3,
    }

    public enum CatanBuildingType
    {
        None = 0,
        Road = 1,
        Settlement = 2,
        City = 3
    }

    public enum CatanPortType
    {
        None = 0,
        Wood = 1,
        Brick = 2,
        Sheep = 3,
        Wheat = 4,
        Ore = 5,
        ThreeToOne = 6
    }

    public enum CatanDevelopmentCardType
    {
        None = 0,
        Knight = 1,
        RoadBuilding = 2,
        YearOfPlenty = 3,
        Monopoly = 4,
        VictoryPoint = 5
    }
}