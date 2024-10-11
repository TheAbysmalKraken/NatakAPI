using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public interface IStateManagerDto<in TStateManager, out TStateManagerDto>
    : IAbstractDto<TStateManager, TStateManagerDto>
{
}