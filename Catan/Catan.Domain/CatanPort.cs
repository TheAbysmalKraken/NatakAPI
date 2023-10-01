namespace Catan.Domain
{
    internal sealed class CatanPort
    {
        private readonly CatanPortType type;
        private readonly Coordinates coordinates;

        public CatanPort(CatanPortType type, Coordinates coordinates)
        {
            this.type = type;
            this.coordinates = coordinates;
        }
    }

    public enum CatanPortType
    {
        None = 0,
        Wood = 1,
        Brick = 2,
        Sheep = 3,
        Wheat = 4,
        Ore = 5,
        ThreeToOne = 6
    }
}
