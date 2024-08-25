using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.DiscardResources;

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
            return Result.Failure(Errors.GameNotFound);
        }

        var catanResources = request.Resources
            .ToDictionary(kvp =>
                kvp.Key, kvp => kvp.Value);

        var discardSuccess = game.DiscardResources(
            (PlayerColour)request.PlayerColour,
            catanResources);

        if (!discardSuccess)
        {
            return Result.Failure(Errors.CannotDiscardResources);
        }

        game.TryFinishDiscardingResources();

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
