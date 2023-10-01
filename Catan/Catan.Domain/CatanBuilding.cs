namespace Catan.Domain
{
    internal sealed class CatanBuilding
    {
        private readonly CatanPlayerColour colour;
        private CatanBuildingType type;

        public CatanBuilding(CatanPlayerColour colour, CatanBuildingType type)
        {
            this.colour = colour;
            this.type = type;
        }
    }

    public enum CatanPlayerColour
    {
        None = 0,
        Red = 1,
        Blue = 2,
        Green = 3,
        Yellow = 4
    }

    public enum CatanBuildingType
    {
        None = 0,
        Road = 1,
        Settlement = 2,
        City = 3
    }
}
