using static Common.Enumerations;

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
}
