using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.CancelTradeOffer;

public sealed class CancelTradeOfferCommandHandler(IActiveGameCache cache) :
    ICommandHandler<CancelTradeOfferCommand>
{
    public async Task<Result> Handle(
        CancelTradeOfferCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        game.CancelTradeOffer();

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
