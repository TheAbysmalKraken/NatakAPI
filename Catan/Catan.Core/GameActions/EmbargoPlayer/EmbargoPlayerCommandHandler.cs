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
            return Result.Failure(GeneralErrors.GameNotFound);
        }

        var playerColour = (PlayerColour)request.PlayerColour;
        var playerColourToEmbargo = (PlayerColour)request.PlayerColourToEmbargo;

        var result = game.EmbargoPlayer(
            playerColour,
            playerColourToEmbargo);

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
