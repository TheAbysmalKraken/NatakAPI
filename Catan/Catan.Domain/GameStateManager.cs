using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Domain;

public sealed class GameStateManager(GameState initialState = GameState.FirstSettlement)
{
    private readonly Dictionary<StateTransition, StateTransitionOutput> transitions = new()
    {
        { new(GameState.FirstSettlement, ActionType.BuildSettlement), new(GameState.FirstRoad) },
        { new(GameState.FirstRoad, ActionType.BuildRoad), new(GameState.FirstSetupReadyForNextPlayer) },
        { new(GameState.FirstSetupReadyForNextPlayer, ActionType.EndTurn), new(GameState.FirstSettlement) },
        { new(GameState.FirstSetupReadyForNextPlayer, ActionType.FirstSetupFinished), new(GameState.SecondSettlement) },
        { new(GameState.SecondSettlement, ActionType.BuildSettlement), new(GameState.SecondRoad) },
        { new(GameState.SecondRoad, ActionType.BuildRoad), new(GameState.SecondSetupReadyForNextPlayer) },
        { new(GameState.SecondSetupReadyForNextPlayer, ActionType.EndTurn), new(GameState.SecondSettlement) },
        { new(GameState.SecondSetupReadyForNextPlayer, ActionType.SecondSetupFinished), new(GameState.BeforeRoll) },
        { new(GameState.BeforeRoll, ActionType.RollDice), new(GameState.AfterRoll) },
        { new(GameState.BeforeRoll, ActionType.RollSeven), new(GameState.DiscardResources, StateTransitionType.Add) },
        { new(GameState.BeforeRoll, ActionType.PlayKnightCard), new(GameState.MoveRobber, StateTransitionType.Add) },
        { new(GameState.BeforeRoll, ActionType.PlayRoadBuildingCard), new(GameState.BeforeRoll) },
        { new(GameState.BeforeRoll, ActionType.PlayYearOfPlentyCard), new(GameState.BeforeRoll) },
        { new(GameState.BeforeRoll, ActionType.PlayMonopolyCard), new(GameState.BeforeRoll) },
        { new(GameState.AfterRoll, ActionType.EndTurn), new(GameState.BeforeRoll) },
        { new(GameState.AfterRoll, ActionType.PlayerHasWon), new(GameState.GameOver) },
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
        { new(GameState.StealResource, ActionType.StealResource), new(GameState.StealResource, StateTransitionType.Remove)}
    };

    private readonly Stack<GameState> stateStack = new([initialState]);

    public GameState CurrentState => stateStack.First();

    public List<ActionType> GetValidActions()
    {
        return transitions
            .Where(kvp => kvp.Key.CurrentState == CurrentState)
            .Select(kvp => kvp.Key.Action)
            .ToList();
    }

    public Result MoveState(ActionType action)
    {
        var nextState = GetNextState(action);

        if (nextState is null)
        {
            return Result.Failure(GameStateManagerErrors.InvalidAction);
        }

        switch (nextState.Type)
        {
            case StateTransitionType.Add:
                stateStack.Push(nextState.OutputState);
                break;

            case StateTransitionType.Remove:
                stateStack.Pop();
                break;

            case StateTransitionType.Keep:
            default:
                stateStack.Pop();
                stateStack.Push(nextState.OutputState);
                break;
        }

        if (stateStack.Count == 0)
        {
            throw new Exception("Gamestate missing.");
        }

        return Result.Success();
    }

    private StateTransitionOutput? GetNextState(ActionType action)
    {
        var transition = new StateTransition(CurrentState, action);
        return transitions.TryGetValue(transition, out var nextState)
            ? nextState
            : null;
    }

    internal sealed record StateTransition(GameState CurrentState, ActionType Action)
    {
        public override int GetHashCode() => (CurrentState, Action).GetHashCode();
    }

    internal sealed record StateTransitionOutput(
        GameState OutputState,
        StateTransitionType Type = StateTransitionType.Keep);

    internal enum StateTransitionType
    {
        Add,
        Keep,
        Remove
    }
}
