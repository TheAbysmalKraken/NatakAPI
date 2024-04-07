namespace Catan.Common;

public static class Enumerations
{
    public enum ResourceType
    {
        None = 0,
        Wood = 1,
        Brick = 2,
        Sheep = 3,
        Wheat = 4,
        Ore = 5,
        Desert = 6
    }

    public enum PlayerColour
    {
        None = -1,
        Red = 0,
        Blue = 1,
        Green = 2,
        Yellow = 3,
    }

    public enum BuildingType
    {
        None = 0,
        Road = 1,
        Settlement = 2,
        City = 3
    }

    public enum PortType
    {
        None = 0,
        Wood = 1,
        Brick = 2,
        Sheep = 3,
        Wheat = 4,
        Ore = 5,
        ThreeToOne = 6
    }

    public enum DevelopmentCardType
    {
        None = 0,
        Knight = 1,
        RoadBuilding = 2,
        YearOfPlenty = 3,
        Monopoly = 4,
        VictoryPoint = 5
    }

    public enum GamePhase
    {
        FirstRoundSetup = 0,
        SecondRoundSetup = 1,
        Main = 2
    }

    public enum GameSubPhase
    {
        BuildSettlement = 0,
        BuildRoad = 1,
        RollOrPlayDevelopmentCard = 2,
        Roll = 3,
        TradeOrBuild = 4,
        PlayTurn = 5,
        MoveRobberSevenRoll = 6,
        MoveRobberKnightCardBeforeRoll = 7,
        MoveRobberKnightCardAfterRoll = 8,
        StealResourceSevenRoll = 9,
        StealResourceKnightCardBeforeRoll = 10,
        StealResourceKnightCardAfterRoll = 11,
        DiscardResources = 12,
        BuildFirstRoadBuildingRoad = 13,
        BuildSecondRoadBuildingRoad = 14
    }
}