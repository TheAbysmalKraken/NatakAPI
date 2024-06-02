using Catan.Application.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.StealResource;

internal sealed class StealResourceCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<StealResourceCommand>
{
    public async Task<Result> Handle(
        StealResourceCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(Errors.GameNotFound);
        }

        if (!game.ContainsPlayer(request.VictimColour))
        {
            return Result.Failure(Errors.InvalidPlayerColour);
        }

        if (game.GameSubPhase != GameSubPhase.StealResourceSevenRoll
        && game.GameSubPhase != GameSubPhase.StealResourceKnightCardBeforeRoll
        && game.GameSubPhase != GameSubPhase.StealResourceKnightCardAfterRoll)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        var stealSuccess = game.StealResourceCard(
            (PlayerColour)request.VictimColour);

        if (!stealSuccess)
        {
            return Result.Failure(Errors.CannotStealResource);
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
