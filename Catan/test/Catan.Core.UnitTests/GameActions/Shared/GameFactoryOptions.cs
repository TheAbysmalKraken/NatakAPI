namespace Catan.Core.UnitTests.GameActions.Shared;

internal sealed class GameFactoryOptions
{
    public int PlayerCount { get; init; } = 4;

    public bool IsSetup { get; init; } = true;
}
