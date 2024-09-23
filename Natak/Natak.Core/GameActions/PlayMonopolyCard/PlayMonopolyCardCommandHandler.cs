using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Enums;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.PlayMonopolyCard;

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
            return Result.Failure(GameErrors.GameNotFound);
        }

        var playCardResult = game.PlayMonopolyCard(
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
