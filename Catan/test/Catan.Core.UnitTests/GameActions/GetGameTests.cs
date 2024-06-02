using Catan.Core.GameActions.GetGame;
using Catan.Core.Services;
using Catan.Domain;

namespace Catan.Core.UnitTests.GameActions;

public sealed class GetGameTests
{
    private static readonly GetGameQuery query = new("testId", 0);

    private readonly GetGameQueryHandler handler;
    private readonly IActiveGameCache cacheMock = Substitute.For<IActiveGameCache>();

    public GetGameTests()
    {
        handler = new GetGameQueryHandler(cacheMock);

        var game = GameData.Create(4);
        SetupCacheGet(game);
    }

    [Fact]
    public async Task Handle_Should_ReturnGame_WhenGameExists()
    {
        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenGameDoesNotExist()
    {
        // Arrange
        SetupCacheGet(null);

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
        var invalidQuery = new GetGameQuery(query.GameId, -1);

        // Act
        var result = await handler.Handle(invalidQuery, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }

    private void SetupCacheGet(Game? game)
    {
        cacheMock
            .GetAsync(query.GameId)
            .Returns(game);
    }
}
