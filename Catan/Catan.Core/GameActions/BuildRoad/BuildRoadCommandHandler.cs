using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
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

        bool buildSuccess;

        if (game.CurrentState == GameState.FirstRoad
        || game.CurrentState == GameState.SecondRoad)
        {
            buildSuccess = game.BuildFreeRoad(request.FirstPoint, request.SecondPoint);
        }
        else
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
