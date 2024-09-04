using Catan.Domain.Enums;

namespace Catan.Domain.Managers;

public sealed class GameStateManager : StateManager
{
    protected override GameState InitialState => GameState.BeforeRoll;

    protected override Dictionary<StateTransition, StateTransitionOutput> Transitions => new()
    {
        { new(GameState.BeforeRoll, ActionType.RollDice), new(GameState.AfterRoll) },
        { new(GameState.BeforeRoll, ActionType.RollSeven), new(GameState.DiscardResources, StateTransitionType.Add) },
        { new(GameState.BeforeRoll, ActionType.PlayKnightCard), new(GameState.MoveRobber, StateTransitionType.Add) },
        { new(GameState.BeforeRoll, ActionType.PlayRoadBuildingCard), new(GameState.BeforeRoll) },
        { new(GameState.BeforeRoll, ActionType.PlayYearOfPlentyCard), new(GameState.BeforeRoll) },
        { new(GameState.BeforeRoll, ActionType.PlayMonopolyCard), new(GameState.BeforeRoll) },
        { new(GameState.AfterRoll, ActionType.EndTurn), new(GameState.BeforeRoll) },
        { new(GameState.AfterRoll, ActionType.PlayerHasWon), new(GameState.Finish) },
        { new(GameState.AfterRoll, ActionType.BuildSettlement), new(GameState.AfterRoll) },
        { new(GameState.AfterRoll, ActionType.BuildRoad), new(GameState.AfterRoll) },
        { new(GameState.AfterRoll, ActionType.BuildCity), new(GameState.AfterRoll) },
        { new(GameState.AfterRoll, ActionType.Trade), new(GameState.AfterRoll) },
        { new(GameState.AfterRoll, ActionType.PlayKnightCard), new(GameState.MoveRobber, StateTransitionType.Add) },
        { new(GameState.AfterRoll, ActionType.PlayRoadBuildingCard), new(GameState.AfterRoll) },
        { new(GameState.AfterRoll, ActionType.PlayYearOfPlentyCard), new(GameState.AfterRoll) },
        { new(GameState.AfterRoll, ActionType.PlayMonopolyCard), new(GameState.AfterRoll) },
        { new(GameState.DiscardResources, ActionType.AllResourcesDiscarded), new(GameState.MoveRobber) },
        { new(GameState.MoveRobber, ActionType.MoveRobber), new(GameState.StealResource) },
        { new(GameState.MoveRobber, ActionType.PlayerHasWon), new(GameState.Finish) },
        { new(GameState.StealResource, ActionType.StealResource), new(GameState.StealResource, StateTransitionType.Remove)}
    };
}
