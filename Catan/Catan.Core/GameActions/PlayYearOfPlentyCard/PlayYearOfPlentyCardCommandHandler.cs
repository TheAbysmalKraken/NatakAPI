using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Core.GameActions.PlayYearOfPlentyCard;

internal sealed class PlayYearOfPlentyCardCommandHandler(
    IActiveGameCache cache)
    : ICommandHandler<PlayYearOfPlentyCardCommand>
{
    public async Task<Result> Handle(
        PlayYearOfPlentyCardCommand request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        var result = game.PlayYearOfPlentyCard(
            (ResourceType)request.FirstResource,
            (ResourceType)request.SecondResource);

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
