using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Domain.Managers;

public abstract class StateManager
{
    protected readonly Stack<GameState> stateStack = new();

    protected abstract GameState InitialState { get; }
    protected abstract Dictionary<StateTransition, StateTransitionOutput> Transitions { get; }

    public StateManager()
    {
        stateStack.Push(InitialState);
    }

    public GameState CurrentState => stateStack.First();

    public List<ActionType> GetValidActions()
    {
        return Transitions
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

        return DoMoveState();
    }

    protected virtual Result DoMoveState()
    {
        return Result.Success();
    }

    private StateTransitionOutput? GetNextState(ActionType action)
    {
        var transition = new StateTransition(CurrentState, action);
        return Transitions.TryGetValue(transition, out var nextState)
            ? nextState
            : null;
    }

    protected sealed record StateTransition(GameState CurrentState, ActionType Action)
    {
        public override int GetHashCode() => (CurrentState, Action).GetHashCode();
    }

    protected sealed record StateTransitionOutput(
        GameState OutputState,
        StateTransitionType Type = StateTransitionType.Keep);

    protected enum StateTransitionType
    {
        Add,
        Keep,
        Remove
    }
}
