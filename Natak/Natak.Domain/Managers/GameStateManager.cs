using Natak.Domain.Enums;

namespace Natak.Domain.Managers;

public sealed class GameStateManager : StateManager
{
    protected override GameState InitialState => GameState.BeforeRoll;

    protected override Dictionary<StateTransition, StateTransitionOutput> Transitions => new()
    {
        { new(GameState.BeforeRoll, ActionType.RollDice), new(GameState.AfterRoll) },
        { new(GameState.BeforeRoll, ActionType.RollSeven), new(GameState.DiscardResources, StateTransitionType.Add) },
        { new(GameState.BeforeRoll, ActionType.PlaySoldierCard), new(GameState.MoveThief, StateTransitionType.Add) },
        { new(GameState.BeforeRoll, ActionType.PlayRoamingCard), new(GameState.Roaming, StateTransitionType.Add) },
        { new(GameState.BeforeRoll, ActionType.PlayWealthCard), new(GameState.BeforeRoll) },
        { new(GameState.BeforeRoll, ActionType.PlayGathererCard), new(GameState.BeforeRoll) },
        { new(GameState.BeforeRoll, ActionType.PlayerHasWon), new(GameState.Finish) },
        { new(GameState.AfterRoll, ActionType.EndTurn), new(GameState.BeforeRoll) },
        { new(GameState.AfterRoll, ActionType.PlayerHasWon), new(GameState.Finish) },
        { new(GameState.AfterRoll, ActionType.BuildVillage), new(GameState.AfterRoll) },
        { new(GameState.AfterRoll, ActionType.BuildRoad), new(GameState.AfterRoll) },
        { new(GameState.AfterRoll, ActionType.BuildTown), new(GameState.AfterRoll) },
        { new(GameState.AfterRoll, ActionType.Trade), new(GameState.AfterRoll) },
        { new(GameState.AfterRoll, ActionType.PlaySoldierCard), new(GameState.MoveThief, StateTransitionType.Add) },
        { new(GameState.AfterRoll, ActionType.PlayRoamingCard), new(GameState.Roaming, StateTransitionType.Add) },
        { new(GameState.AfterRoll, ActionType.PlayWealthCard), new(GameState.AfterRoll) },
        { new(GameState.AfterRoll, ActionType.PlayGathererCard), new(GameState.AfterRoll) },
        { new(GameState.DiscardResources, ActionType.AllResourcesDiscarded), new(GameState.MoveThief) },
        { new(GameState.MoveThief, ActionType.MoveThief), new(GameState.StealResource) },
        { new(GameState.MoveThief, ActionType.PlayerHasWon), new(GameState.Finish) },
        { new(GameState.StealResource, ActionType.StealResource), new(GameState.StealResource, StateTransitionType.Remove) },
        { new(GameState.Roaming, ActionType.BuildRoad), new(GameState.Roaming) },
        { new(GameState.Roaming, ActionType.FinishRoaming), new(GameState.Roaming, StateTransitionType.Remove) }
    };
}
