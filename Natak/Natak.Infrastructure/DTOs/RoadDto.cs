using Natak.Domain;

namespace Natak.Infrastructure.DTOs;

public sealed class RoadDto : BuildingDto, IDto<Road, RoadDto>
{
    public required PointDto FirstPoint { get; init; }
    
    public required PointDto SecondPoint { get; init; }
    
    public static RoadDto FromDomain(Road domain)
    {
        return new RoadDto()
        {
            Colour = domain.Colour,
            Type = domain.Type,
            FirstPoint = PointDto.FromDomain(domain.FirstPoint),
            SecondPoint = PointDto.FromDomain(domain.SecondPoint)
        };
    }

    public new Road ToDomain()
    {
        return new Road(
            Colour,
            FirstPoint.ToDomain(),
            SecondPoint.ToDomain());
    }
}