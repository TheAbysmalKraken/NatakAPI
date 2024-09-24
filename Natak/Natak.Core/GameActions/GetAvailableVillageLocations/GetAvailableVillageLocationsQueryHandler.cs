using Natak.Core.Abstractions;
using Natak.Core.Models;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.GetAvailableVillageLocations;

internal sealed class GetAvailableVillageLocationsQueryHandler(
    IActiveGameCache cache)
    : IQueryHandler<GetAvailableVillageLocationsQuery, List<PointResponse>>
{
    public async Task<Result<List<PointResponse>>> Handle(
        GetAvailableVillageLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure<List<PointResponse>>(GameErrors.GameNotFound);
        }

        var result = game.GetAvailableVillageLocations();

        if (result.IsFailure)
        {
            return Result.Failure<List<PointResponse>>(result.Error);
        }

        var availableVillagePoints = result.Value;

        var response = availableVillagePoints.Select(PointResponse.FromPoint).ToList();

        return Result.Success(response);
    }
}
