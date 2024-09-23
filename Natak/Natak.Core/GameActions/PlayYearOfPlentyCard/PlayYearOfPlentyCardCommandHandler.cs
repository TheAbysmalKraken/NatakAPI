using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Enums;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.PlayYearOfPlentyCard;

internal sealed class PlayYearOfPlentyCardCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<PlayYearOfPlentyCardCommand>
{
    public async Task<Result> Handle(
        PlayYearOfPlentyCardCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        var playCardResult = game.PlayYearOfPlentyCard(
            (ResourceType)request.FirstResource,
            (ResourceType)request.SecondResource);

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
