using Catan.Core.GameActions.GetAvailableCityLocations;
using Catan.Core.Services;
using Catan.Core.UnitTests.GameActions.Shared;
using Catan.Domain.Enums;

namespace Catan.Core.UnitTests.GameActions;

public sealed class GetAvailableCityLocationsTests
{
    private static readonly GetAvailableCityLocationsQuery query = new("testId", (int)PlayerColour.Red);

    private readonly GetAvailableCityLocationsQueryHandler handler;
    private readonly IActiveGameCache cacheMock = Substitute.For<IActiveGameCache>();

    public GetAvailableCityLocationsTests()
    {
        handler = new GetAvailableCityLocationsQueryHandler(cacheMock);

        var game = GameData.Create(4);
        ActiveGameCacheMocker.SetupGetAsyncMock(cacheMock, game);
    }

    [Fact]
    public async Task Handle_Should_ReturnPoints_WhenPlayerHasSettlements()
    {
        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenGameDoesNotExist()
    {
        // Arrange
        ActiveGameCacheMocker.SetupGetAsyncMock(cacheMock, null);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenPlayerColourIsInvalid()
    {
        // Arrange
        var invalidQuery = new GetAvailableCityLocationsQuery(query.GameId, (int)PlayerColour.None);

        // Act
        var result = await handler.Handle(invalidQuery, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }
}
