using static Catan.Common.Enumerations;

namespace Catan.Domain;

public class Road(
    PlayerColour colour,
    Point firstCornerCoordinates,
    Point secondCornerCoordinates) : Building(colour, BuildingType.Road)
{
    public Point FirstCornerCoordinates { get; private set; } = firstCornerCoordinates;

    public Point SecondCornerCoordinates { get; private set; } = secondCornerCoordinates;
}
