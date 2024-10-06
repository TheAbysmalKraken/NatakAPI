using Natak.Domain.Enums;

namespace Natak.Domain;

public class Building(PlayerColour colour, BuildingType type)
{
    public Building() : this(PlayerColour.None, BuildingType.None)
    {
    }

    public PlayerColour Colour { get; private set; } = colour;

    public BuildingType Type { get; private set; } = type;

    public void SetColour(PlayerColour colour)
    {
        Colour = colour;
    }

    public void SetTypeToHouse()
    {
        Type = BuildingType.Village;
    }

    public void SetTypeToTown()
    {
        Type = BuildingType.Town;
    }
}
