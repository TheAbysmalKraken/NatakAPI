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

        var result = game.RollDiceAndDistributeResourcesToPlayers();

        if (result.IsFailure)
        {
            return Result.Failure<RollDiceResponse>(result.Error);
        }

        await cache.UpsetAsync(
            game.Id,
            game,
            cancellationToken: cancellationToken);

        return Result.Success(new RollDiceResponse()
        {
            RolledDice = game.LastRoll
        });
    }
}
