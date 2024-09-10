using Catan.Core.GameActions.GetAvailableCityLocations;
using Catan.Core.Services;
using Catan.Core.UnitTests.GameActions.Shared;
using Catan.Domain.Factories;

namespace Catan.Core.UnitTests.GameActions;

public sealed class GetAvailableCityLocationsTests
{
    private static readonly GetAvailableCityLocationsQuery query = new("testId");

    private readonly GetAvailableCityLocationsQueryHandler handler;
    private readonly IActiveGameCache cacheMock = Substitute.For<IActiveGameCache>();

    public GetAvailableCityLocationsTests()
    {
        handler = new GetAvailableCityLocationsQueryHandler(cacheMock);

        var game = GameFactory.Create();
        ActiveGameCacheMocker.SetupGetAsyncMock(cacheMock, game);
    }

    [Fact]
    public async Task Handle_Should_ReturnPoints_WhenPlayerHasSettlements()
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
