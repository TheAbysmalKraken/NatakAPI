using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.EndTurn;

internal sealed class EndTurnCommandHandler(IActiveGameCache cache) :
    ICommandHandler<EndTurnCommand>
{
    public async Task<Result> Handle(EndTurnCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(Errors.GameNotFound);
        }

        if (game.GameSubPhase != GameSubPhase.PlayTurn
        && game.GameSubPhase != GameSubPhase.TradeOrBuild)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        game.NextPlayer();
        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
