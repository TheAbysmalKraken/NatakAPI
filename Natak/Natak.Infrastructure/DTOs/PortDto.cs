using Natak.Domain;
using Natak.Domain.Enums;

namespace Natak.Infrastructure.DTOs;

public sealed class PortDto : IDto<Port, PortDto>
{
    public required PortType Type { get; init; }
    
    public required PointDto Point { get; init; }
    
    public static PortDto FromDomain(Port domain)
    {
        return new PortDto()
        {
            Type = domain.Type,
            Point = PointDto.FromDomain(domain.Point)
        };
    }

    public Port ToDomain()
    {
        return new Port(Type, Point.ToDomain());
    }
}