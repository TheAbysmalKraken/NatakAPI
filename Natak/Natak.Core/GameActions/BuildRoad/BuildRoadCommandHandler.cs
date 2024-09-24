using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Enums;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.BuildRoad;

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
