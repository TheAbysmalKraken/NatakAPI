﻿using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

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
            return Result.Failure(Errors.GameNotFound);
        }

        if (game.GameSubPhase != GameSubPhase.TradeOrBuild)
        {
            return Result.Failure(Errors.InvalidGamePhase);
        }

        var tradeSuccess = game.MakeTradeOffer(
            request.Offer,
            request.Request);

        if (!tradeSuccess)
        {
            return Result.Failure(Errors.CannotMakeTradeOffer);
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}