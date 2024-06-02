using Catan.Application.Models;
using Catan.Core.Services;
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

        var playerColourToStealFrom = request.PlayerColourToStealFrom;

        if (!game.ContainsPlayer(playerColourToStealFrom))
        {
            return Result.Failure<PlayerSpecificGameStatusResponse>(Errors.InvalidPlayerColour);
        }

        if (game.GameSubPhase != GameSubPhase.PlayTurn
        && game.GameSubPhase != GameSubPhase.TradeOrBuild
        && game.GameSubPhase != GameSubPhase.RollOrPlayDevelopmentCard)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        if (game.HasPlayedDevelopmentCardThisTurn)
        {
            return Result.Failure(Errors.AlreadyPlayedDevelopmentCard);
        }

        var playSuccess = game.PlayKnightCard(request.MoveRobberTo, (PlayerColour)playerColourToStealFrom);

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
