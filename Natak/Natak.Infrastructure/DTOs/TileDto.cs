using Natak.Domain;
using Natak.Domain.Enums;

namespace Natak.Infrastructure.DTOs;

public sealed class TileDto : IDto<Tile, TileDto>
{
    public required ResourceType Type { get; init; }
    
    public required int ActivationNumber { get; init; }
    
    public required PointDto Point { get; init; }
    
    public static TileDto FromDomain(Tile domain)
    {
        return new TileDto()
        {
            Type = domain.Type,
            ActivationNumber = domain.ActivationNumber,
            Point = PointDto.FromDomain(domain.Point)
        };
    }

    public Tile ToDomain()
    {
        return new Tile(
            Type,
            ActivationNumber,
            Point.ToDomain());
    }
}