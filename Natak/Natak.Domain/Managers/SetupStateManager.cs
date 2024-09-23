using Natak.Domain.Enums;

namespace Natak.Domain.Managers;

public sealed class SetupStateManager : StateManager
{
    protected override GameState InitialState => GameState.InitialVillage;

    protected override Dictionary<StateTransition, StateTransitionOutput> Transitions => new()
    {
        { new(GameState.InitialVillage, ActionType.BuildVillage), new(GameState.InitialRoad) },
        { new(GameState.InitialRoad, ActionType.BuildRoad), new(GameState.SetupFinished) },
        { new(GameState.SetupFinished, ActionType.EndTurn), new(GameState.InitialVillage) }
    };
}
