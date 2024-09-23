using Natak.Core.Abstractions;
using Natak.Core.Models;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.GetAvailableTownLocations;

internal sealed class GetAvailableTownLocationsQueryHandler(
    IActiveGameCache cache)
    : IQueryHandler<GetAvailableTownLocationsQuery, List<PointResponse>>
{
    public async Task<Result<List<PointResponse>>> Handle(
        GetAvailableTownLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure<List<PointResponse>>(GameErrors.GameNotFound);
        }

        var result = game.GetAvailableTownLocations();

        if (result.IsFailure)
        {
            return Result.Failure<List<PointResponse>>(result.Error);
        }

        var availableTownPoints = result.Value;

        var response = availableTownPoints.Select(PointResponse.FromPoint).ToList();

        return Result.Success(response);
    }
}
