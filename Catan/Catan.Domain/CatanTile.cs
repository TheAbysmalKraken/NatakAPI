using static Catan.Common.Enumerations;

namespace Catan.Domain
{
    public sealed class CatanTile
    {
        private readonly CatanResourceType type;
        private readonly int activationNumber;

        public CatanTile(CatanResourceType type, int activationNumber)
        {
            this.type = type;
            this.activationNumber = activationNumber;
        }

        public CatanResourceType GetResourceType()
        {
            return type;
        }

        public int GetActivationNumber()
        {
            return activationNumber;
        }
    }
}
