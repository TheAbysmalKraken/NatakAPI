using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.EmbargoPlayer;

internal sealed class EmbargoPlayerCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<EmbargoPlayerCommand>
{
    public async Task<Result> Handle(
        EmbargoPlayerCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(Errors.GameNotFound);
        }

        var playerColour = (PlayerColour)request.PlayerColour;
        var playerColourToEmbargo = (PlayerColour)request.PlayerColourToEmbargo;

        if (!game.ContainsPlayer(playerColour)
        || !game.ContainsPlayer(playerColourToEmbargo))
        {
            return Result.Failure(Errors.InvalidPlayerColour);
        }

        var embargoSuccess = game.EmbargoPlayer(
            playerColour,
            playerColourToEmbargo);

        if (!embargoSuccess)
        {
            return Result.Failure(Errors.CannotEmbargoPlayer);
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
