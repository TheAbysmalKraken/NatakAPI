using Natak.Domain.Enums;

namespace Natak.Domain;

public class House(
    PlayerColour colour,
    HouseType type,
    Point point)
{
    public House(Point point)
        : this(PlayerColour.None, HouseType.None, point)
    {
    }

    public PlayerColour Colour { get; private set; } = colour;

    public HouseType Type { get; private set; } = type;

    public Point Point { get; private set; } = point;

    public void SetColour(PlayerColour colour)
    {
        Colour = colour;
    }
    
    public void SetTypeToVillage()
    {
        Type = HouseType.Village;
    }
    
    public void SetTypeToTown()
    {
        Type = HouseType.Town;
    }
}
