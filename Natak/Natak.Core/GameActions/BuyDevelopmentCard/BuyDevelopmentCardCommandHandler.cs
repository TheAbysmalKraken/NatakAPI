using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.BuyGrowthCard;

internal sealed class BuyGrowthCardCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<BuyGrowthCardCommand>
{
    public async Task<Result> Handle(
        BuyGrowthCardCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        var purchaseResult = game.BuyGrowthCard();

        if (purchaseResult.IsFailure)
        {
            return purchaseResult;
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
