using static Catan.Common.Enumerations;

namespace Catan.Domain
{
    public class CatanRoad : CatanBuilding
    {
        public Coordinates FirstCornerCoordinates { get; }
        public Coordinates SecondCornerCoordinates { get; }

        public CatanRoad(CatanPlayerColour colour, Coordinates firstCornerCoordinates, Coordinates secondCornerCoordinates)
        {
            this.Colour = colour;
            this.Type = CatanBuildingType.Road;
            this.FirstCornerCoordinates = firstCornerCoordinates;
            this.SecondCornerCoordinates = secondCornerCoordinates;
        }
    }
}
