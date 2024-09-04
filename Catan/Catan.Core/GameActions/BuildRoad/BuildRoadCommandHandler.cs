using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Core.GameActions.BuildRoad;

internal sealed class BuildRoadCommandHandler(IActiveGameCache cache) :
    ICommandHandler<BuildRoadCommand>
{
    public async Task<Result> Handle(BuildRoadCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        if (!game.IsSetup)
        {
            var purchaseResult = game.BuyRoad();

            if (purchaseResult.IsFailure)
            {
                return purchaseResult;
            }
        }

        var buildRoadResult = game.PlaceRoad(
            request.FirstPoint,
            request.SecondPoint);

        if (buildRoadResult.IsFailure)
        {
            return buildRoadResult;
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
