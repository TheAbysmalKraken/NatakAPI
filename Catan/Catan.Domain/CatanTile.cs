using static Catan.Common.Enumerations;

namespace Catan.Domain
{
    public sealed class CatanTile
    {
        public CatanResourceType Type { get; }
        public int ActivationNumber { get; }

        public CatanTile(CatanResourceType type, int activationNumber)
        {
            this.Type = type;
            this.ActivationNumber = activationNumber;
        }
    }
}
