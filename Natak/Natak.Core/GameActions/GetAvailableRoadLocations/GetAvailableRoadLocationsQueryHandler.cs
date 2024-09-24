using Natak.Core.Abstractions;
using Natak.Core.Models;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Enums;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.GetAvailableRoadLocations;

internal sealed class GetAvailableRoadLocationsQueryHandler(
    IActiveGameCache cache)
    : IQueryHandler<GetAvailableRoadLocationsQuery, List<RoadResponse>>
{
    public async Task<Result<List<RoadResponse>>> Handle(
        GetAvailableRoadLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure<List<RoadResponse>>(GameErrors.GameNotFound);
        }

        var board = game.Board;

        var result = game.GetAvailableRoadLocations();

        if (result.IsFailure)
        {
            return Result.Failure<List<RoadResponse>>(result.Error);
        }

        var availableRoadPoints = result.Value;

        var response = availableRoadPoints.Select(RoadResponse.FromDomain).ToList();

        return Result.Success(response);
    }
}
