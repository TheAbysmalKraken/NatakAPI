using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.StealResource;

internal sealed class StealResourceCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<StealResourceCommand>
{
    public async Task<Result> Handle(
        StealResourceCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GeneralErrors.GameNotFound);
        }

        var result = game.StealResourceCard(
            (PlayerColour)request.VictimColour);

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
