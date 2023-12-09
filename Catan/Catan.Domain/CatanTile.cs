using static Catan.Common.Enumerations;

namespace Catan.Domain;

public sealed class CatanTile
{
    public CatanTile(CatanResourceType type, int activationNumber)
    {
        Type = type;
        ActivationNumber = activationNumber;
    }

    public CatanResourceType Type { get; private set; }
    
    public int ActivationNumber { get; private set; }
}
