using static Catan.Common.Enumerations;

namespace Catan.Domain;

public class CatanRoad : CatanBuilding
{
    public CatanRoad(
        CatanPlayerColour colour,
        Coordinates firstCornerCoordinates,
        Coordinates secondCornerCoordinates)
        : base(colour, CatanBuildingType.Road)
    {
        FirstCornerCoordinates = firstCornerCoordinates;
        SecondCornerCoordinates = secondCornerCoordinates;
    }

    public Coordinates FirstCornerCoordinates { get; private set; }

    public Coordinates SecondCornerCoordinates { get; private set; }
}
