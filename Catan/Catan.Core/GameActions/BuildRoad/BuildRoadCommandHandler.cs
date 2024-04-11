using Catan.Application.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.BuildRoad;

internal sealed class BuildRoadCommandHandler(IActiveGameCache cache) :
    ICommandHandler<BuildRoadCommand>
{
    public async Task<Result> Handle(BuildRoadCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(Errors.GameNotFound);
        }

        if (game.GameSubPhase != GameSubPhase.BuildRoad
        && game.GameSubPhase != GameSubPhase.PlayTurn
        && game.GameSubPhase != GameSubPhase.TradeOrBuild)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        bool buildSuccess = false;

        if (game.GamePhase == GamePhase.FirstRoundSetup
        || game.GamePhase == GamePhase.SecondRoundSetup)
        {
            buildSuccess = game.BuildFreeRoad(request.FirstPoint, request.SecondPoint);
        }
        else if (game.GamePhase == GamePhase.Main)
        {
            buildSuccess = game.BuildRoad(request.FirstPoint, request.SecondPoint);
        }

        if (!buildSuccess)
        {
            return Result.Failure(Errors.InvalidBuildLocation);
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
