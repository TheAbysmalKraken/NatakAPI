using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.PlayKnightCard;

internal sealed class PlayKnightCardCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<PlayKnightCardCommand>
{
    public async Task<Result> Handle(
        PlayKnightCardCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(Errors.GameNotFound);
        }

        var playerColourToStealFrom = (PlayerColour)request.PlayerColourToStealFrom;

        if (!game.ContainsPlayer(playerColourToStealFrom))
        {
            return Result.Failure(Errors.InvalidPlayerColour);
        }

        if (game.HasPlayedDevelopmentCardThisTurn)
        {
            return Result.Failure(Errors.AlreadyPlayedDevelopmentCard);
        }

        var playSuccess = game.PlayKnightCard(
            request.MoveRobberTo,
            playerColourToStealFrom);

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
