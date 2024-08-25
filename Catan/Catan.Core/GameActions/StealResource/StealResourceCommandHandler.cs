using Catan.Core.Models;
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

        var victimColour = (PlayerColour)request.VictimColour;

        if (!game.ContainsPlayer(victimColour))
        {
            return Result.Failure(Errors.InvalidPlayerColour);
        }

        var stealSuccess = game.StealResourceCard(
            victimColour);

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
