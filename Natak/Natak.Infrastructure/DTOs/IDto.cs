using Natak.Domain;

namespace Natak.Infrastructure.DTOs;

public interface IDto<TDomain, out TOutput> :
    IFromDomainMappable<TDomain, TOutput>,
    IToDomainMappable<TDomain>
{
}