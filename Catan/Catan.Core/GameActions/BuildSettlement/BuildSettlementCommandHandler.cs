using Catan.Domain;
using Catan.Core.Services;
using Catan.Domain.Enums;
using Catan.Core.Abstractions;

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

        bool buildSuccess;

        if (game.CurrentState == GameState.FirstSettlement
        || game.CurrentState == GameState.SecondSettlement)
        {
            buildSuccess = game.BuildFreeSettlement(request.BuildPoint);
        }
        else
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
