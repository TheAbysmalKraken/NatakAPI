namespace Natak.Domain;

public interface IToDomainMappable<out TDomain>
{
    TDomain ToDomain();
}