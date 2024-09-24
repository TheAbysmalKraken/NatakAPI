using Natak.Domain;
using Natak.Core.Services;
using Natak.Core.Abstractions;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.BuildVillage;

internal sealed class BuildVillageCommandHandler(IActiveGameCache cache) :
    ICommandHandler<BuildVillageCommand>
{
    public async Task<Result> Handle(BuildVillageCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        if (!game.IsSetup)
        {
            var purchaseResult = game.BuyVillage();

            if (purchaseResult.IsFailure)
            {
                return purchaseResult;
            }
        }

        var buildVillageResult = game.PlaceVillage(request.BuildPoint);

        if (buildVillageResult.IsFailure)
        {
            return buildVillageResult;
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
