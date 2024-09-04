using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;
using Catan.Domain.Errors;

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
            return Result.Failure(GameErrors.GameNotFound);
        }

        if (game.DevelopmentCardPlayed)
        {
            return Result.Failure(PlayerErrors.DevelopmentCardAlreadyPlayed);
        }

        var playCardResult = game.PlayRoadBuildingCard(
            request.FirstRoadFirstPoint,
            request.FirstRoadSecondPoint,
            request.SecondRoadFirstPoint,
            request.SecondRoadSecondPoint);

        if (playCardResult.IsFailure)
        {
            return playCardResult;
        }

        var removeCardResult = game.RemoveDevelopmentCardFromCurrentPlayer(
            DevelopmentCardType.RoadBuilding);

        if (removeCardResult.IsFailure)
        {
            return removeCardResult;
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
