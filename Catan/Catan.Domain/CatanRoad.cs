using static Catan.Common.Enumerations;

namespace Catan.Domain;

public class CatanRoad(
    CatanPlayerColour colour,
    Coordinates firstCornerCoordinates,
    Coordinates secondCornerCoordinates) : CatanBuilding(colour, CatanBuildingType.Road)
{
    public Coordinates FirstCornerCoordinates { get; private set; } = firstCornerCoordinates;

    public Coordinates SecondCornerCoordinates { get; private set; } = secondCornerCoordinates;
}
