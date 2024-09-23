using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.BuildTown;

internal sealed class BuildTownCommandHandler(IActiveGameCache cache) :
    ICommandHandler<BuildTownCommand>
{
    public async Task<Result> Handle(BuildTownCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        var purchaseResult = game.BuyTown();

        if (purchaseResult.IsFailure)
        {
            return purchaseResult;
        }

        var buildTownResult = game.PlaceTown(
            request.BuildPoint);

        if (buildTownResult.IsFailure)
        {
            return buildTownResult;
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
