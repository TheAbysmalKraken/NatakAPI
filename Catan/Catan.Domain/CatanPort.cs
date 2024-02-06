using static Catan.Common.Enumerations;

namespace Catan.Domain;

public sealed class CatanPort(CatanPortType type, Coordinates coordinates)
{
    public CatanPortType Type { get; private set; } = type;

    public Coordinates Coordinates { get; private set; } = coordinates;
}
