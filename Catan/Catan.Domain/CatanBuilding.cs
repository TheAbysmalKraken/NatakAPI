using static Catan.Common.Enumerations;

namespace Catan.Domain
{
    public class CatanBuilding
    {
        public CatanPlayerColour Colour { get; set; }
        public CatanBuildingType Type { get; set; }

        public CatanBuilding()
        {
            this.Colour = CatanPlayerColour.None;
            this.Type = CatanBuildingType.None;
        }

        public CatanBuilding(CatanPlayerColour colour, CatanBuildingType type)
        {
            this.Colour = colour;
            this.Type = type;
        }
    }
}
