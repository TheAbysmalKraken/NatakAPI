using static Catan.Common.Enumerations;

namespace Catan.Domain;

public sealed class Port(PortType type, Point coordinates)
{
    public PortType Type { get; private set; } = type;

    public Point Coordinates { get; private set; } = coordinates;
}
