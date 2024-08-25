using Catan.Core.Abstractions;
using Catan.Core.Services;
using Catan.Domain;

namespace Catan.Core.GameActions.BuildCity;

internal sealed class BuildCityCommandHandler(IActiveGameCache cache) :
    ICommandHandler<BuildCityCommand>
{
    public async Task<Result> Handle(BuildCityCommand request, CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure(GeneralErrors.GameNotFound);
        }

        var result = game.BuildCity(request.BuildPoint);

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
