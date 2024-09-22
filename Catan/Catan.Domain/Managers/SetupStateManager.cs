using Catan.Domain.Enums;

namespace Catan.Domain.Managers;

public sealed class SetupStateManager : StateManager
{
    protected override GameState InitialState => GameState.InitialSettlement;

    protected override Dictionary<StateTransition, StateTransitionOutput> Transitions => new()
    {
        { new(GameState.InitialSettlement, ActionType.BuildSettlement), new(GameState.InitialRoad) },
        { new(GameState.InitialRoad, ActionType.BuildRoad), new(GameState.SetupFinished) },
        { new(GameState.SetupFinished, ActionType.EndTurn), new(GameState.InitialSettlement) }
    };
}
