using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public sealed class SetupStateManagerDto :
    StateManagerDto,
    IStateManagerDto<SetupStateManager, SetupStateManagerDto>
{
    public static SetupStateManagerDto FromDomain(SetupStateManager domain)
    {
        return new SetupStateManagerDto()
        {
            StateStack = domain.GetStateStack()
        };
    }

    public override SetupStateManager ToDomain()
    {
        return new SetupStateManager(StateStack);
    }
}