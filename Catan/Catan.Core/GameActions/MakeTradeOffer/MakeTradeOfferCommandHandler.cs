using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;

namespace Catan.Core.GameActions.MakeTradeOffer;

internal sealed class MakeTradeOfferCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<MakeTradeOfferCommand>
{
    public async Task<Result> Handle(
        MakeTradeOfferCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GeneralErrors.GameNotFound);
        }

        var result = game.MakeTradeOffer(
            request.Offer,
            request.Request);

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
