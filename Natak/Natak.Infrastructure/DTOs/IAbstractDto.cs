using Natak.Domain;

namespace Natak.Infrastructure.DTOs;

public interface IAbstractDto<in TDomain, out TDto> :
    IFromDomainMappable<TDomain, TDto>
{
}