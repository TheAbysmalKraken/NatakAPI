using static Common.Enumerations;

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
}
