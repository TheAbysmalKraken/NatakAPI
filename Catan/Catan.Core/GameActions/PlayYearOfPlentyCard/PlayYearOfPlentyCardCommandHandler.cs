using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.PlayYearOfPlentyCard;

internal sealed class PlayYearOfPlentyCardCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<PlayYearOfPlentyCardCommand>
{
    public async Task<Result> Handle(
        PlayYearOfPlentyCardCommand request,
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

        var playSuccess = game.PlayYearOfPlentyCard(
            (ResourceType)request.FirstResource,
            (ResourceType)request.SecondResource);

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
