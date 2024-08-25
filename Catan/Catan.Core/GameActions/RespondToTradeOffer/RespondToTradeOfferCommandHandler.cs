using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
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
            return Result.Failure(GeneralErrors.GameNotFound);
        }

        var playerColour = (PlayerColour)request.PlayerColour;

        if (request.Accept)
        {
            var acceptResult = game.AcceptTradeOffer(playerColour);

            if (acceptResult.IsFailure)
            {
                return acceptResult;
            }
        }
        else
        {
            var rejectResult = game.RejectTradeOffer(playerColour);

            if (rejectResult.IsFailure)
            {
                return rejectResult;
            }
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
