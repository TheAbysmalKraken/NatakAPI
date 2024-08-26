namespace Catan.Domain.Enums;

public enum GameState
{
    FirstSettlement,
    FirstRoad,
    FirstSetupReadyForNextPlayer,
    SecondSettlement,
    SecondRoad,
    SecondSetupReadyForNextPlayer,
    BeforeRoll,
    AfterRoll,
    DiscardResources,
    MoveRobber,
    GameOver
}
