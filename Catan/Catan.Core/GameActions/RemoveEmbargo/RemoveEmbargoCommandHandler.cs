using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Core.GameActions.RemoveEmbargo;

public sealed class RemoveEmbargoCommandHandler(IActiveGameCache cache) :
    ICommandHandler<RemoveEmbargoCommand>
{
    public async Task<Result> Handle(
        RemoveEmbargoCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        var playerColour = (PlayerColour)request.PlayerColour;
        var playerColourToRemoveEmbargoOn = (PlayerColour)request.PlayerColourToRemoveEmbargoOn;

        var result = game.RemoveEmbargo(
            playerColour,
            playerColourToRemoveEmbargoOn);

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
