using Catan.Domain;
using Catan.Core.Services;
using Catan.Domain.Enums;
using Catan.Core.Abstractions;
using Catan.Domain.Errors;

namespace Catan.Core.GameActions.BuildSettlement;

internal sealed class BuildSettlementCommandHandler(IActiveGameCache cache) :
    ICommandHandler<BuildSettlementCommand>
{
    public async Task<Result> Handle(BuildSettlementCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        if (!game.IsSetup)
        {
            var purchaseResult = game.BuySettlement();

            if (purchaseResult.IsFailure)
            {
                return purchaseResult;
            }
        }

        var buildSettlementResult = game.PlaceSettlement(request.BuildPoint);

        if (buildSettlementResult.IsFailure)
        {
            return buildSettlementResult;
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
