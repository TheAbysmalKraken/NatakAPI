using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

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
            return Result.Failure(Errors.GameNotFound);
        }

        if (game.HasPlayedDevelopmentCardThisTurn)
        {
            return Result.Failure(Errors.AlreadyPlayedDevelopmentCard);
        }

        var playSuccess = game.PlayRoadBuildingCard(
            request.FirstRoadFirstPoint,
            request.FirstRoadSecondPoint,
            request.SecondRoadFirstPoint,
            request.SecondRoadSecondPoint);

        if (!playSuccess)
        {
            return Result.Failure(Errors.CannotPlayDevelopmentCard);
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
