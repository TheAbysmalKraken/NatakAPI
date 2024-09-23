using Natak.Core.Services;
using Natak.Domain;

namespace Natak.Core.UnitTests.GameActions.Shared;

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
