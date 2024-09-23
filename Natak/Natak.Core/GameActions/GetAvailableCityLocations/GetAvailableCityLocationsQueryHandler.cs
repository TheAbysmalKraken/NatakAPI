using Natak.Core.Abstractions;
using Natak.Core.Models;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Enums;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.GetAvailableCityLocations;

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
