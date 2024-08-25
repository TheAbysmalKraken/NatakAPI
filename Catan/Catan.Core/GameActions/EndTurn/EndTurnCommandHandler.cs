using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;

namespace Catan.Core.GameActions.EndTurn;

internal sealed class EndTurnCommandHandler(IActiveGameCache cache) :
    ICommandHandler<EndTurnCommand>
{
    public async Task<Result> Handle(EndTurnCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(Errors.GameNotFound);
        }

        game.NextPlayer();
        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
