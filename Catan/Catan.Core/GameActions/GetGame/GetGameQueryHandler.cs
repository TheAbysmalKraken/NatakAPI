using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.GetGame;

internal sealed class GetGameQueryHandler(IActiveGameCache cache) :
    IQueryHandler<GetGameQuery, GameResponse>
{
    public async Task<Result<GameResponse>> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure<GameResponse>(Errors.GameNotFound);
        }
        if (!game.ContainsPlayer((PlayerColour)request.PlayerColour))
        {
            return Result.Failure<GameResponse>(Errors.InvalidPlayerColour);
        }

        var response = GameResponse.FromDomain(game, request.PlayerColour);

        return Result.Success(response);
    }
}
