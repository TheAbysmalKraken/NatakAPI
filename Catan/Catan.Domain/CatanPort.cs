using static Catan.Common.Enumerations;

namespace Catan.Domain;

public sealed class CatanPort
{
    public CatanPort(CatanPortType type, Coordinates coordinates)
    {
        Type = type;
        Coordinates = coordinates;
    }
    public CatanPortType Type { get; private set; }

    public Coordinates Coordinates { get; private set; }
}
