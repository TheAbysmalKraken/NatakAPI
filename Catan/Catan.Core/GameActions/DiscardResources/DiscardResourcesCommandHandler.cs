using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;
using Catan.Domain.Errors;

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
            return Result.Failure(GameErrors.GameNotFound);
        }

        var catanResources = request.Resources
            .ToDictionary(kvp =>
                kvp.Key, kvp => kvp.Value);

        var result = game.DiscardResources(
            (PlayerColour)request.PlayerColour,
            catanResources);

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
