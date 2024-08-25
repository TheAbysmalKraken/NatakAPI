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
            return Result.Failure(GeneralErrors.GameNotFound);
        }

        Result result;

        if (game.CurrentState == GameState.FirstRoad
        || game.CurrentState == GameState.SecondRoad)
        {
            result = game.BuildRoad(request.FirstPoint, request.SecondPoint, true);
        }
        else
        {
            result = game.BuildRoad(request.FirstPoint, request.SecondPoint);
        }

        if (result.IsFailure)
        {
            return result;
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
