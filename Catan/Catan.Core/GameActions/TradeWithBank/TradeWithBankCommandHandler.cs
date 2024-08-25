using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.TradeWithBank;

internal sealed class TradeWithBankCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<TradeWithBankCommand>
{
    public async Task<Result> Handle(
        TradeWithBankCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(Errors.GameNotFound);
        }

        var resourceToGive = (ResourceType)request.ResourceToGive;
        var resourceToGet = (ResourceType)request.ResourceToGet;

        var tradeTwoToOneSuccess = game.TradeTwoToOne(
            resourceToGive,
            resourceToGet);

        if (tradeTwoToOneSuccess)
        {
            await cache.UpsetAsync(
                request.GameId,
                game,
                cancellationToken: cancellationToken);
            return Result.Success();
        }

        var tradeThreeToOneSuccess = game.TradeThreeToOne(
            resourceToGive,
            resourceToGet);

        if (tradeThreeToOneSuccess)
        {
            await cache.UpsetAsync(
                request.GameId,
                game,
                cancellationToken: cancellationToken);
            return Result.Success();
        }

        var tradeFourToOneSuccess = game.TradeFourToOne(
            resourceToGive,
            resourceToGet);

        if (tradeFourToOneSuccess)
        {
            await cache.UpsetAsync(
                request.GameId,
                game,
                cancellationToken: cancellationToken);
            return Result.Success();
        }

        return Result.Failure(Errors.CannotTradeWithBank);
    }
}
