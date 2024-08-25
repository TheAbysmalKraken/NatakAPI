using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;

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
            return Result.Failure(GeneralErrors.GameNotFound);
        }

        var result = game.MoveRobber(request.MoveRobberTo);

        if (result.IsFailure)
        {
            return Result.Failure(GeneralErrors.CannotMoveRobberToLocation);
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
