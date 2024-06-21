using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.RespondToTradeOffer;

internal sealed class RespondToTradeOfferCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<RespondToTradeOfferCommand>
{
    public async Task<Result> Handle(
        RespondToTradeOfferCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(Errors.GameNotFound);
        }

        if (!game.TradeOffer.IsActive)
        {
            return Result.Failure(Errors.NoTradeOfferToRespondTo);
        }

        if (game.GameSubPhase != GameSubPhase.TradeOrBuild)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        var playerColour = (PlayerColour)request.PlayerColour;

        if (request.Accept)
        {
            var acceptSuccess = game.AcceptTradeOffer(playerColour);

            if (!acceptSuccess)
            {
                return Result.Failure(Errors.CannotRespondToTradeOffer);
            }
        }
        else
        {
            var rejectSuccess = game.RejectTradeOffer(playerColour);

            if (!rejectSuccess)
            {
                return Result.Failure(Errors.CannotRespondToTradeOffer);
            }
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
