using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.PlaySoldierCard;

internal sealed class PlaySoldierCardCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<PlaySoldierCardCommand>
{
    public async Task<Result> Handle(
        PlaySoldierCardCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        var playCardResult = game.PlaySoldierCard();

        if (playCardResult.IsFailure)
        {
            return playCardResult;
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
