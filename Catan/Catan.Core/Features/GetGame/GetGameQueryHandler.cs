using Catan.Application.Models;
using Catan.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Catan.Core.Features.GetGame;

public sealed class GetGameQueryHandler(IMemoryCache cache) :
    IRequestHandler<GetGameQuery, Result<PlayerSpecificGameStatusResponse>>
{
    public Task<Result<PlayerSpecificGameStatusResponse>> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        var gameResult = GetGameFromCache(request.GameId);

        if (gameResult.IsFailure)
        {
            return Task.FromResult(Result.Failure<PlayerSpecificGameStatusResponse>(gameResult.Error));
        }

        var game = gameResult.Value;

        if (!IsValidPlayerColour(request.PlayerColour, game.PlayerCount))
        {
            return Task.FromResult(Result.Failure<PlayerSpecificGameStatusResponse>(Errors.InvalidPlayerColour));
        }

        var response = PlayerSpecificGameStatusResponse.FromDomain(game, request.PlayerColour);

        return Task.FromResult(Result.Success(response));
    }

    private Result<Game> GetGameFromCache(string gameId)
    {
        var game = cache.Get<Game>(gameId);

        if (game is null)
        {
            return Result.Failure<Game>(Errors.GameNotFound);
        }

        return Result.Success(game);
    }

    private static bool IsValidPlayerColour(int colour, int playerCount)
    {
        return colour >= 0 && colour < playerCount;
    }

    private void SetGameInCache(Game game)
    {
        cache.Set(game.Id, game, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
        });
    }
}
