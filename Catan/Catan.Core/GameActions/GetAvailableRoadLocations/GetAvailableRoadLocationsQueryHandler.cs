using Catan.Core.Abstractions;
using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;
using Catan.Domain.Errors;

namespace Catan.Core.GameActions.GetAvailableRoadLocations;

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
