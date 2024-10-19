using Natak.Domain.Enums;

namespace Natak.Domain;

public sealed class Tile(
    ResourceType type,
    int activationNumber,
    Point point)
{
    public ResourceType Type { get; private set; } = type;

    public int ActivationNumber { get; private set; } = activationNumber;
    
    public Point Point { get; private set; } = point;
}
