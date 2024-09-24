using Natak.Core.GameActions.GetAvailableTownLocations;
using Natak.Core.Services;
using Natak.Core.UnitTests.GameActions.Shared;
using Natak.Domain.Factories;

namespace Natak.Core.UnitTests.GameActions;

public sealed class GetAvailableTownLocationsTests
{
    private static readonly GetAvailableTownLocationsQuery query = new("testId");

    private readonly GetAvailableTownLocationsQueryHandler handler;
    private readonly IActiveGameCache cacheMock = Substitute.For<IActiveGameCache>();

    public GetAvailableTownLocationsTests()
    {
        handler = new GetAvailableTownLocationsQueryHandler(cacheMock);

        var game = GameFactory.Create();
        ActiveGameCacheMocker.SetupGetAsyncMock(cacheMock, game);
    }

    [Fact]
    public async Task Handle_Should_ReturnPoints_WhenPlayerHasVillages()
    {
        // Arrange
        var gameOptions = new GameFactoryOptions
        {
            IsSetup = false
        };

        var game = GameFactory.Create(gameOptions);
        ActiveGameCacheMocker.SetupGetAsyncMock(cacheMock, game);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}
