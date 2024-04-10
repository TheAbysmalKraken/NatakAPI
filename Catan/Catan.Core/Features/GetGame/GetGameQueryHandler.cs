using Catan.Application.Models;
using Catan.Core.Services;
using MediatR;

namespace Catan.Core.Features.GetGame;

public sealed class GetGameQueryHandler(IActiveGameCache cache) :
    IRequestHandler<GetGameQuery, Result<PlayerSpecificGameStatusResponse>>
{
    public async Task<Result<PlayerSpecificGameStatusResponse>> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure<PlayerSpecificGameStatusResponse>(Errors.GameNotFound);
        }

        if (!IsValidPlayerColour(request.PlayerColour, game.PlayerCount))
        {
            return Result.Failure<PlayerSpecificGameStatusResponse>(Errors.InvalidPlayerColour);
        }

        var response = PlayerSpecificGameStatusResponse.FromDomain(game, request.PlayerColour);

        return Result.Success(response);
    }

    private static bool IsValidPlayerColour(int colour, int playerCount)
    {
        return colour >= 0 && colour < playerCount;
    }
}
