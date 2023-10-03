using static Catan.Common.Enumerations;

namespace Catan.Domain
{
    public sealed class CatanPort
    {
        public CatanPortType Type { get; }
        public Coordinates Coordinates { get; }

        public CatanPort(CatanPortType type, Coordinates coordinates)
        {
            this.Type = type;
            this.Coordinates = coordinates;
        }
    }
}
