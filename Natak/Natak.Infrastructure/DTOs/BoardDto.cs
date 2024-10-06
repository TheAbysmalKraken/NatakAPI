using Natak.Domain;

namespace Natak.Infrastructure.DTOs;

public sealed class BoardDto : IDto<Board, BoardDto>
{
    public required TileManagerDto TileManager { get; init; }
    
    public required Building[,] Houses { get; init; }
    
    public required List<PortDto> Ports { get; init; }
    
    public required List<RoadDto> Roads { get; init; }
    
    public required PointDto ThiefPosition { get; init; }
    
    public required LongestRoadInfoDto LongestRoadInfo { get; init; }
    
    public static BoardDto FromDomain(Board domain)
    {
        return new BoardDto()
        {
            TileManager = TileManagerDto.FromDomain(domain.GetTileManager()),
            Houses = domain.GetHouses(),
            Ports = domain.GetPorts().Select(PortDto.FromDomain).ToList(),
            Roads = domain.GetRoads().Select(RoadDto.FromDomain).ToList(),
            ThiefPosition = PointDto.FromDomain(domain.ThiefPosition),
            LongestRoadInfo = LongestRoadInfoDto.FromDomain(domain.LongestRoadInfo)
        };
    }

    public Board ToDomain()
    {
        return new Board(
            TileManager.ToDomain(),
            Houses,
            Ports.Select(p => p.ToDomain()).ToList(),
            Roads.Select(r => r.ToDomain()).ToList(),
            ThiefPosition.ToDomain(),
            LongestRoadInfo.ToDomain());
    }
}