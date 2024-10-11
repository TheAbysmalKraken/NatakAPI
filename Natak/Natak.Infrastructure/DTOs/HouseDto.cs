using Natak.Domain;
using Natak.Domain.Enums;

namespace Natak.Infrastructure.DTOs;

public sealed class HouseDto : IDto<House, HouseDto>
{
    public required PlayerColour Colour { get; init; }
    
    public required HouseType Type { get; init; }
    
    public required PointDto Point { get; init; }
    
    public static HouseDto FromDomain(House domain)
    {
        return new HouseDto()
        {
            Colour = domain.Colour,
            Type = domain.Type,
            Point = PointDto.FromDomain(domain.Point)
        };
    }

    public House ToDomain()
    {
        return new House(
            Colour,
            Type,
            Point.ToDomain());
    }
}