namespace Natak.Domain;

public interface IFromDomainMappable<in TDomain, out TOutput>
{
    static abstract TOutput FromDomain(TDomain domain);
}