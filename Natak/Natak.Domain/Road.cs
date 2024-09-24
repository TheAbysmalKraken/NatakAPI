using Natak.Domain.Enums;

namespace Natak.Domain;

public class Road(
    PlayerColour colour,
    Point firstPoint,
    Point secondPoint) : Building(colour, BuildingType.Road)
{
    public Point FirstPoint { get; private set; } = firstPoint;

    public Point SecondPoint { get; private set; } = secondPoint;
}
