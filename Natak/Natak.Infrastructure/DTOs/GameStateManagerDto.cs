using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public sealed class GameStateManagerDto :
    StateManagerDto,
    IStateManagerDto<GameStateManager, GameStateManagerDto>
{
    public static GameStateManagerDto FromDomain(GameStateManager domain)
    {
        return new GameStateManagerDto()
        {
            StateStack = domain.GetStateStack()
        };
    }

    public override GameStateManager ToDomain()
    {
        return new GameStateManager(StateStack);
    }
}