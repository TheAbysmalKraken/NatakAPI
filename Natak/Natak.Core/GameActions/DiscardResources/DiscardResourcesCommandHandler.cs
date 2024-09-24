using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Enums;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.DiscardResources;

internal sealed class DiscardResourcesCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<DiscardResourcesCommand>
{
    public async Task<Result> Handle(
        DiscardResourcesCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        var resourcesToDiscard = request.Resources
            .ToDictionary(kvp =>
                kvp.Key, kvp => kvp.Value);

        var player = game.GetPlayer((PlayerColour)request.PlayerColour);

        if (player is null)
        {
            return Result.Failure(PlayerErrors.NotFound);
        }

        var resourcesToDiscardCount = resourcesToDiscard.Values.Sum();

        if (player.CardsToDiscard != resourcesToDiscardCount)
        {
            return Result.Failure(PlayerErrors.IncorrectDiscardCount);
        }

        var discardResult = game.DiscardResources(
            player,
            resourcesToDiscard);

        if (discardResult.IsFailure)
        {
            return discardResult;
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
