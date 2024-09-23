using Natak.Domain;
using Natak.Core.Services;
using Natak.Domain.Enums;
using Natak.Core.Abstractions;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.BuildSettlement;

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
