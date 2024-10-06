using Natak.Domain;
using Natak.Domain.Enums;

namespace Natak.Infrastructure.DTOs;

public class BuildingDto : IDto<Building, BuildingDto>
{
    public required PlayerColour Colour { get; init; }
    
    public required BuildingType Type { get; init; }
    
    public static BuildingDto FromDomain(Building domain)
    {
        return new BuildingDto()
        {
            Colour = domain.Colour,
            Type = domain.Type
        };
    }

    public Building ToDomain()
    {
        return new Building(Colour, Type);
    }
}