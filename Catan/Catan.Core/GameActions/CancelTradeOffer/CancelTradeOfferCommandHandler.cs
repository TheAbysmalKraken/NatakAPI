using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;

namespace Catan.Core.GameActions.CancelTradeOffer;

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
            return Result.Failure(GeneralErrors.GameNotFound);
        }

        var result = game.CancelTradeOffer();

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
