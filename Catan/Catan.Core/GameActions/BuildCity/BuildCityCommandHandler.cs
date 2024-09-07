using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Errors;

namespace Catan.Core.GameActions.BuildCity;

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
