using Natak.Domain;
using Natak.Domain.Enums;

namespace Natak.Infrastructure.DTOs;

public sealed class RoadDto : IDto<Road, RoadDto>
{
    public required PlayerColour Colour { get; init; }
    
    public required PointDto FirstPoint { get; init; }
    
    public required PointDto SecondPoint { get; init; }
    
    public static RoadDto FromDomain(Road domain)
    {
        return new RoadDto()
        {
            Colour = domain.Colour,
            FirstPoint = PointDto.FromDomain(domain.FirstPoint),
            SecondPoint = PointDto.FromDomain(domain.SecondPoint)
        };
    }

    public Road ToDomain()
    {
        return new Road(
            Colour,
            FirstPoint.ToDomain(),
            SecondPoint.ToDomain());
    }
}