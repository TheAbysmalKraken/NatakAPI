using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Domain;

public sealed class GameStateManager()
{
    private readonly Dictionary<StateTransition, GameState> transitions = new()
    {
        { new StateTransition(GameState.FirstSettlement, ActionType.BuildSettlement), GameState.FirstRoad },
        { new StateTransition(GameState.FirstSettlement, ActionType.FirstSetupFinished), GameState.SecondSettlement },
        { new StateTransition(GameState.FirstRoad, ActionType.BuildRoad), GameState.FirstSettlement },
        { new StateTransition(GameState.SecondSettlement, ActionType.BuildSettlement), GameState.SecondRoad },
        { new StateTransition(GameState.SecondSettlement, ActionType.SecondSetupFinished), GameState.BeforeRoll },
        { new StateTransition(GameState.SecondRoad, ActionType.BuildRoad), GameState.SecondSettlement },
        { new StateTransition(GameState.BeforeRoll, ActionType.RollDice), GameState.AfterRoll },
        { new StateTransition(GameState.BeforeRoll, ActionType.RollSeven), GameState.DiscardResources },
        { new StateTransition(GameState.BeforeRoll, ActionType.PlayKnightCard), GameState.BeforeRoll },
        { new StateTransition(GameState.BeforeRoll, ActionType.PlayRoadBuildingCard), GameState.BeforeRoll },
        { new StateTransition(GameState.BeforeRoll, ActionType.PlayYearOfPlentyCard), GameState.BeforeRoll },
        { new StateTransition(GameState.BeforeRoll, ActionType.PlayMonopolyCard), GameState.BeforeRoll },
        { new StateTransition(GameState.AfterRoll, ActionType.EndTurn), GameState.BeforeRoll },
        { new StateTransition(GameState.AfterRoll, ActionType.PlayerHasWon), GameState.GameOver },
        { new StateTransition(GameState.AfterRoll, ActionType.BuildSettlement), GameState.AfterRoll },
        { new StateTransition(GameState.AfterRoll, ActionType.BuildRoad), GameState.AfterRoll },
        { new StateTransition(GameState.AfterRoll, ActionType.BuildCity), GameState.AfterRoll },
        { new StateTransition(GameState.AfterRoll, ActionType.Trade), GameState.AfterRoll },
        { new StateTransition(GameState.AfterRoll, ActionType.PlayKnightCard), GameState.AfterRoll },
        { new StateTransition(GameState.AfterRoll, ActionType.PlayRoadBuildingCard), GameState.AfterRoll },
        { new StateTransition(GameState.AfterRoll, ActionType.PlayYearOfPlentyCard), GameState.AfterRoll },
        { new StateTransition(GameState.AfterRoll, ActionType.PlayMonopolyCard), GameState.AfterRoll },
        { new StateTransition(GameState.DiscardResources, ActionType.AllResourcesDiscarded), GameState.MoveRobber },
        { new StateTransition(GameState.MoveRobber, ActionType.MoveRobber), GameState.AfterRoll }
    };

    public GameState CurrentState { get; private set; } = GameState.FirstSettlement;

    public List<ActionType> GetValidActions()
    {
        return transitions
            .Where(kvp => kvp.Key.CurrentState == CurrentState)
            .Select(kvp => kvp.Key.Action)
            .ToList();
    }

    public GameState? GetNextState(ActionType action)
    {
        var transition = new StateTransition(CurrentState, action);
        return transitions.TryGetValue(transition, out var nextState)
            ? nextState
            : null;
    }

    public Result MoveState(ActionType action)
    {
        var nextState = GetNextState(action);

        if (nextState is null)
        {
            return Result.Failure(GameStateManagerErrors.InvalidAction);
        }

        CurrentState = nextState.Value;

        return Result.Success();
    }

    internal sealed record StateTransition(GameState CurrentState, ActionType Action)
    {
        public override int GetHashCode() => (CurrentState, Action).GetHashCode();
    }
}
