using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.MoveRobber;

internal sealed class MoveRobberCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<MoveRobberCommand>
{
    public async Task<Result> Handle(
        MoveRobberCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(Errors.GameNotFound);
        }

        var moveSuccess = game.MoveRobber(request.MoveRobberTo);

        if (!moveSuccess)
        {
            return Result.Failure(Errors.CannotMoveRobberToLocation);
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
