using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

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
            return Result.Failure(Errors.GameNotFound);
        }

        var buySuccess = game.BuyDevelopmentCard();

        if (!buySuccess)
        {
            return Result.Failure(Errors.CannotBuyDevelopmentCard);
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
