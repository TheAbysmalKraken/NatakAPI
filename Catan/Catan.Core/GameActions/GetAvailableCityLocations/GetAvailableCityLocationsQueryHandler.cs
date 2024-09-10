using Catan.Core.Abstractions;
using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Core.GameActions.GetAvailableCityLocations;

internal sealed class GetAvailableCityLocationsQueryHandler(
    IActiveGameCache cache)
    : IQueryHandler<GetAvailableCityLocationsQuery, List<PointResponse>>
{
    public async Task<Result<List<PointResponse>>> Handle(
        GetAvailableCityLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure<List<PointResponse>>(GameErrors.GameNotFound);
        }

        var result = game.GetAvailableCityLocations();

        if (result.IsFailure)
        {
            return Result.Failure<List<PointResponse>>(result.Error);
        }

        var availableCityPoints = result.Value;

        var response = availableCityPoints.Select(PointResponse.FromPoint).ToList();

        return Result.Success(response);
    }
}
