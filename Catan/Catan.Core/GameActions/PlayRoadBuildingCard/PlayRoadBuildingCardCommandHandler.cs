using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;

namespace Catan.Core.GameActions.PlayRoadBuildingCard;

internal sealed class PlayRoadBuildingCardCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<PlayRoadBuildingCardCommand>
{
    public async Task<Result> Handle(
        PlayRoadBuildingCardCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GeneralErrors.GameNotFound);
        }

        var result = game.PlayRoadBuildingCard(
            request.FirstRoadFirstPoint,
            request.FirstRoadSecondPoint,
            request.SecondRoadFirstPoint,
            request.SecondRoadSecondPoint);

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
