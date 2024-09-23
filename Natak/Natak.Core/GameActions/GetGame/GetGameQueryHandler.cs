using Natak.Core.Abstractions;
using Natak.Core.Models;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Enums;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.GetGame;

internal sealed class GetGameQueryHandler(IActiveGameCache cache) :
    IQueryHandler<GetGameQuery, GameResponse>
{
    public async Task<Result<GameResponse>> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure<GameResponse>(GameErrors.GameNotFound);
        }

        if (game.GetPlayer((PlayerColour)request.PlayerColour) is null)
        {
            return Result.Failure<GameResponse>(PlayerErrors.NotFound);
        }

        var response = GameResponse.FromDomain(game, request.PlayerColour);

        return Result.Success(response);
    }
}
