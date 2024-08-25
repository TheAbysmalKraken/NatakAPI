using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.BuildCity;

internal sealed class BuildCityCommandHandler(IActiveGameCache cache) :
    ICommandHandler<BuildCityCommand>
{
    public async Task<Result> Handle(BuildCityCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(Errors.GameNotFound);
        }

        var buildSuccess = game.BuildCity(request.BuildPoint);

        if (!buildSuccess)
        {
            return Result.Failure(Errors.InvalidBuildLocation);
        }

        await cache.UpsetAsync(
            request.GameId,
            game,
            cancellationToken: cancellationToken);

        return Result.Success();
    }
}
