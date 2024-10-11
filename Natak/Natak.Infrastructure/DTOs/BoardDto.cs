using Natak.Domain;
using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public sealed class BoardDto : IDto<Board, BoardDto>
{
    public required TileManagerDto TileManager { get; init; }
    
    public required BuildingManagerDto BuildingManager { get; init; }
    
    public required List<PortDto> Ports { get; init; }
    
    public required PointDto ThiefPosition { get; init; }
    
    public required LongestRoadInfoDto LongestRoadInfo { get; init; }
    
    public static BoardDto FromDomain(Board domain)
    {
        return new BoardDto()
        {
            TileManager = TileManagerDto.FromDomain(domain.GetTileManager()),
            BuildingManager = BuildingManagerDto.FromDomain(domain.GetBuildingManager()),
            Ports = domain.GetPorts().Select(PortDto.FromDomain).ToList(),
            ThiefPosition = PointDto.FromDomain(domain.ThiefPosition),
            LongestRoadInfo = LongestRoadInfoDto.FromDomain(domain.LongestRoadInfo)
        };
    }

    public Board ToDomain()
    {
        return new Board(
            TileManager.ToDomain(),
            BuildingManager.ToDomain(),
            Ports.Select(p => p.ToDomain()).ToList(),
            ThiefPosition.ToDomain(),
            LongestRoadInfo.ToDomain());
    }
}