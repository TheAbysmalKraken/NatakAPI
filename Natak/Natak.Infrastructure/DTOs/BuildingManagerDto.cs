using Natak.Domain.Managers;

namespace Natak.Infrastructure.DTOs;

public sealed class BuildingManagerDto :
    IDto<BuildingManager, BuildingManagerDto>
{
    public required List<HouseDto> Houses { get; init; }
    
    public required List<RoadDto> Roads { get; init; }
    
    public static BuildingManagerDto FromDomain(BuildingManager domain)
    {
        return new BuildingManagerDto()
        {
            Houses = domain.GetHouses().Select(HouseDto.FromDomain).ToList(),
            Roads = domain.GetRoads().Select(RoadDto.FromDomain).ToList()
        };
    }

    public BuildingManager ToDomain()
    {
        return new BuildingManager(
            Houses.Select(h => h.ToDomain()).ToList(),
            Roads.Select(r => r.ToDomain()).ToList());
    }
}