using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Errors;

namespace Catan.Core.GameActions.RollDice;

internal sealed class RollDiceCommandHandler(IActiveGameCache cache) :
    ICommandHandler<RollDiceCommand, RollDiceResponse>
{
    public async Task<Result<RollDiceResponse>> Handle(RollDiceCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure<RollDiceResponse>(
                GameErrors.GameNotFound);
        }

        var rollResult = game.RollDice();

        if (rollResult.IsFailure)
        {
            return Result.Failure<RollDiceResponse>(rollResult.Error);
        }

        var distributeResourcesResult = game.DistributeResources();

        if (distributeResourcesResult.IsFailure)
        {
            return Result.Failure<RollDiceResponse>(distributeResourcesResult.Error);
        }

        await cache.UpsetAsync(
            game.Id,
            game,
            cancellationToken: cancellationToken);

        return Result.Success(new RollDiceResponse()
        {
            RolledDice = game.LastRoll.Outcome
        });
    }
}
