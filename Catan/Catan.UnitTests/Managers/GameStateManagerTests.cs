using Catan.Domain.Enums;
using Catan.Domain.Managers;

namespace Catan.Domain.UnitTests.Managers;

public sealed class GameStateManagerTests
{
    [Fact]
    public void MoveState_Should_TransitionToBeforeRoll_WhenStealingResourceBeforeRoll()
    {
        // Arrange
        var stateManager = new GameStateManager();
        stateManager.MoveState(ActionType.RollSeven);
        stateManager.MoveState(ActionType.AllResourcesDiscarded);
        stateManager.MoveState(ActionType.MoveRobber);

        // Act
        stateManager.MoveState(ActionType.StealResource);

        // Assert
        Assert.Equal(GameState.BeforeRoll, stateManager.CurrentState);
    }

    [Fact]
    public void MoveState_Should_TransitionToAfterRoll_WhenStealingResourceAfterRoll()
    {
        // Arrange
        var stateManager = new GameStateManager();
        stateManager.MoveState(ActionType.RollDice);
        stateManager.MoveState(ActionType.PlayKnightCard);
        stateManager.MoveState(ActionType.MoveRobber);

        // Act
        stateManager.MoveState(ActionType.StealResource);

        // Assert
        Assert.Equal(GameState.AfterRoll, stateManager.CurrentState);
    }
}
