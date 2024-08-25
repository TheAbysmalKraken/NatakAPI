using Catan.Domain;
using Catan.Core.Services;
using Catan.Domain.Enums;
using Catan.Core.Abstractions;

namespace Catan.Core.GameActions.BuildSettlement;

internal sealed class BuildSettlementCommandHandler(IActiveGameCache cache) :
    ICommandHandler<BuildSettlementCommand>
{
    public async Task<Result> Handle(BuildSettlementCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GeneralErrors.GameNotFound);
        }

        Result result;

        if (game.CurrentState == GameState.FirstSettlement
        || game.CurrentState == GameState.SecondSettlement)
        {
            result = game.BuildSettlement(request.BuildPoint, true);
        }
        else
        {
            result = game.BuildSettlement(request.BuildPoint);
        }

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
