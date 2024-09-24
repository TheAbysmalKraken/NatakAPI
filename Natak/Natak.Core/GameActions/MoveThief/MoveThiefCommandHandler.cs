using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.MoveThief;

internal sealed class MoveThiefCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<MoveThiefCommand>
{
    public async Task<Result> Handle(
        MoveThiefCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        var result = game.MoveThief(request.MoveThiefTo);

        if (result.IsFailure)
        {
            return result;
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
