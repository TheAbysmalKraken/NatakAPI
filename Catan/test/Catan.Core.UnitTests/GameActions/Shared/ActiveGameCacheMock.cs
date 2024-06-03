using Catan.Core.Services;
using Catan.Domain;

namespace Catan.Core.UnitTests.GameActions.Shared;

public static class ActiveGameCacheMocker
{
    public static void SetupGetAsyncMock(IActiveGameCache cacheMock, Game? game)
    {
        cacheMock
            .GetAsync(
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(game);
    }
}
