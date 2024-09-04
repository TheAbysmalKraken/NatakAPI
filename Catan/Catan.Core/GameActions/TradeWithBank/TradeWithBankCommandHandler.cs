using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;
using Catan.Domain.Errors;

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
            return Result.Failure(GameErrors.GameNotFound);
        }

        var resourceToGive = (ResourceType)request.ResourceToGive;
        var resourceToGet = (ResourceType)request.ResourceToGet;

        var result = game.TradeWithBank(
            resourceToGive,
            resourceToGet);

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
