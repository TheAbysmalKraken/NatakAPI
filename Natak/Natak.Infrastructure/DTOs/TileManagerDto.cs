using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public sealed class TileManagerDto : IDto<TileManager, TileManagerDto>
{
    public required List<TileDto> Tiles { get; init; }
    
    public static TileManagerDto FromDomain(TileManager domain)
    {
        return new TileManagerDto()
        {
            Tiles = domain.GetTiles()
                .Select(TileDto.FromDomain)
                .ToList()
        };
    }

    public TileManager ToDomain()
    {
        return new TileManager(
            Tiles.Select(t => t.ToDomain())
                .ToList());
    }
}