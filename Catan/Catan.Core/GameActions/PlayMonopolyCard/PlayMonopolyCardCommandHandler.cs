using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Core.GameActions.PlayMonopolyCard;

internal sealed class PlayMonopolyCardCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<PlayMonopolyCardCommand>
{
    public async Task<Result> Handle(
        PlayMonopolyCardCommand request,
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

        var resourceType = (ResourceType)request.Resource;

        if (resourceType == ResourceType.Desert)
        {
            return Result.Failure(GameErrors.InvalidResourceType);
        }

        var playCardResult = game.PlayMonopolyCard(
            (ResourceType)request.Resource);

        if (playCardResult.IsFailure)
        {
            return playCardResult;
        }

        var removeCardResult = game.RemoveDevelopmentCardFromCurrentPlayer(
            DevelopmentCardType.Monopoly);

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
