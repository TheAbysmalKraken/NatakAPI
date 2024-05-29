using Catan.Domain.Enums;

namespace Catan.Domain;

public sealed class Port(PortType type, Point point)
{
    public PortType Type { get; private set; } = type;

    public Point Point { get; private set; } = point;
}
