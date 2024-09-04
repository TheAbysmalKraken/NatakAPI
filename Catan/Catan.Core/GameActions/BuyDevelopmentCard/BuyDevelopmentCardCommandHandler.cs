using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Errors;

namespace Catan.Core.GameActions.BuyDevelopmentCard;

internal sealed class BuyDevelopmentCardCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<BuyDevelopmentCardCommand>
{
    public async Task<Result> Handle(
        BuyDevelopmentCardCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        var purchaseResult = game.BuyDevelopmentCard();

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
