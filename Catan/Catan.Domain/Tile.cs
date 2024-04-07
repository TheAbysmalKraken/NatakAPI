using static Catan.Common.Enumerations;

namespace Catan.Domain;

public sealed class Tile(ResourceType type, int activationNumber)
{
    public ResourceType Type { get; private set; } = type;

    public int ActivationNumber { get; private set; } = activationNumber;
}
