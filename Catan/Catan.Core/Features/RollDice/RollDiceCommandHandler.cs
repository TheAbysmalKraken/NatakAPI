using Catan.Application.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

namespace Catan.Core.Features.RollDice;

internal sealed class RollDiceCommandHandler(IActiveGameCache cache) :
    ICommandHandler<RollDiceCommand, RollDiceResponse>
{
    public async Task<Result<RollDiceResponse>> Handle(RollDiceCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure<RollDiceResponse>(
                Errors.GameNotFound);
        }

        if (game.GamePhase != GamePhase.Main || (game.GameSubPhase != GameSubPhase.RollOrPlayDevelopmentCard
            && game.GameSubPhase != GameSubPhase.Roll))
        {
            return Result.Failure<RollDiceResponse>(
                Errors.InvalidGamePhase);
        }

        game.RollDiceAndDistributeResourcesToPlayers();
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
