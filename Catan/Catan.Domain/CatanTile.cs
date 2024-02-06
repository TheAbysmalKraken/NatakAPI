using static Catan.Common.Enumerations;

namespace Catan.Domain;

public sealed class CatanTile(CatanResourceType type, int activationNumber)
{
    public CatanResourceType Type { get; private set; } = type;

    public int ActivationNumber { get; private set; } = activationNumber;
}
