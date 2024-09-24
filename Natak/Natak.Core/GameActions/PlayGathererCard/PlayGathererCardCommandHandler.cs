using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Enums;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.PlayGathererCard;

internal sealed class PlayGathererCardCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<PlayGathererCardCommand>
{
    public async Task<Result> Handle(
        PlayGathererCardCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        var playCardResult = game.PlayGathererCard(
            (ResourceType)request.Resource);

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
