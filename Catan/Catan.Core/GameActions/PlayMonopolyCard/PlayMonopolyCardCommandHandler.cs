using Catan.Application.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

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
            return Result.Failure(Errors.GameNotFound);
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

        var playSuccess = game.PlayMonopolyCard(
            (ResourceType)request.Resource);

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
