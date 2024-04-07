namespace Catan.Domain.Enums;

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
