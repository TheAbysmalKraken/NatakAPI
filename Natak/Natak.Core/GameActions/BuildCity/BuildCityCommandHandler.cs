using Natak.Core.Abstractions;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.BuildCity;

internal sealed class BuildCityCommandHandler(IActiveGameCache cache) :
    ICommandHandler<BuildCityCommand>
{
    public async Task<Result> Handle(BuildCityCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GameErrors.GameNotFound);
        }

        var purchaseResult = game.BuyCity();

        if (purchaseResult.IsFailure)
        {
            return purchaseResult;
        }

        var buildCityResult = game.PlaceCity(
            request.BuildPoint);

        if (buildCityResult.IsFailure)
        {
            return buildCityResult;
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
