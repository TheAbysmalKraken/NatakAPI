using Catan.Application.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.BuildSettlement;

internal sealed class BuildSettlementCommandHandler(IActiveGameCache cache) :
    ICommandHandler<BuildSettlementCommand>
{
    public async Task<Result> Handle(BuildSettlementCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(Errors.GameNotFound);
        }

        if (game.GameSubPhase != GameSubPhase.BuildSettlement
        && game.GameSubPhase != GameSubPhase.PlayTurn
        && game.GameSubPhase != GameSubPhase.TradeOrBuild)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        bool buildSuccess = false;

        if (game.GamePhase == GamePhase.FirstRoundSetup
        || game.GamePhase == GamePhase.SecondRoundSetup)
        {
            buildSuccess = game.BuildFreeSettlement(request.BuildPoint);
        }
        else if (game.GamePhase == GamePhase.Main)
        {
            buildSuccess = game.BuildSettlement(request.BuildPoint);
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
