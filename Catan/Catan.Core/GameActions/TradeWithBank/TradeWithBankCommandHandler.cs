using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
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
            return Result.Failure(GeneralErrors.GameNotFound);
        }

        var resourceToGive = (ResourceType)request.ResourceToGive;
        var resourceToGet = (ResourceType)request.ResourceToGet;

        var result = game.TradeTwoToOne(
            resourceToGive,
            resourceToGet);

        if (result.IsSuccess)
        {
            await cache.UpsetAsync(
                request.GameId,
                game,
                cancellationToken: cancellationToken);

            return Result.Success();
        }

        var tradeThreeToOneResult = game.TradeThreeToOne(
            resourceToGive,
            resourceToGet);

        if (tradeThreeToOneResult.IsSuccess)
        {
            await cache.UpsetAsync(
                request.GameId,
                game,
                cancellationToken: cancellationToken);

            return Result.Success();
        }

        var tradeFourToOneResult = game.TradeFourToOne(
            resourceToGive,
            resourceToGet);

        if (tradeFourToOneResult.IsSuccess)
        {
            await cache.UpsetAsync(
                request.GameId,
                game,
                cancellationToken: cancellationToken);

            return Result.Success();
        }

        return tradeFourToOneResult;
    }
}
