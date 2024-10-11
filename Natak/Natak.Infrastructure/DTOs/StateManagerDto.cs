using Natak.Domain.Enums;
using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public abstract class StateManagerDto : IStateManagerDto<StateManager, StateManagerDto>
{
    public required Stack<GameState> StateStack { get; init; }
    
    public static StateManagerDto FromDomain(StateManager domain)
    {
        return domain switch
        {
            SetupStateManager setupStateManager => SetupStateManagerDto.FromDomain(setupStateManager),
            GameStateManager gameStateManager => GameStateManagerDto.FromDomain(gameStateManager),
            _ => throw new ArgumentException($"Unknown {nameof(StateManager)} type: {domain.GetType()}")
        };
    }

    public abstract StateManager ToDomain();
}