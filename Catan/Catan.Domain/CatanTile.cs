namespace Catan.Domain
{
    internal sealed class CatanTile
    {
        private readonly CatanResourceType type;
        private readonly int activationNumber;

        public CatanTile(CatanResourceType type, int activationNumber)
        {
            this.type = type;
            this.activationNumber = activationNumber;
        }

        public CatanResourceType GetTileType()
        {
            return type;
        }

        public int GetActivationNumber()
        {
            return activationNumber;
        }
    }

    public enum CatanResourceType
    {
        Unknown = 0,
        Wood = 1,
        Brick = 2,
        Sheep = 3,
        Wheat = 4,
        Ore = 5,
        Desert = 6
    }
}
