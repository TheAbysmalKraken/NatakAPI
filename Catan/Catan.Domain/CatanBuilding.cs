using static Catan.Common.Enumerations;

namespace Catan.Domain
{
    public sealed class CatanBuilding
    {
        private readonly CatanPlayerColour colour;
        private CatanBuildingType type;

        public CatanBuilding(CatanPlayerColour colour, CatanBuildingType type)
        {
            this.colour = colour;
            this.type = type;
        }
    }
}
