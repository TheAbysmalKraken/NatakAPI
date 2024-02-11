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

    public enum CatanGamePhase
    {
        FirstRoundSetup = 0,
        SecondRoundSetup = 1,
        Main = 1
    }

    public enum CatanGameSubPhase
    {
        BuildSettlement = 0,
        BuildRoad = 1,
        RollOrPlayDevelopmentCard = 2,
        Roll = 3,
        TradeOrBuild = 4,
        PlayTurn = 5,
        MoveRobber = 6,
        StealResource = 7
    }
}