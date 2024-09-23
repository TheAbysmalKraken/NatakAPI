using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Enums;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.PlayKnightCard;

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
            return Result.Failure(GameErrors.GameNotFound);
        }

        var playCardResult = game.PlayKnightCard();

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
