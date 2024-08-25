using Catan.Core.Abstractions;
using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;

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
            return Result.Failure<List<RoadResponse>>(Errors.GameNotFound);
        }

        var playerColour = (PlayerColour)request.PlayerColour;
        if (!game.ContainsPlayer(playerColour))
        {
            return Result.Failure<List<RoadResponse>>(Errors.InvalidPlayerColour);
        }

        var board = game.Board;

        var availableRoadPoints = SelectAvailableRoadPoints(
            board,
            playerColour);

        var response = availableRoadPoints.Select(RoadResponse.FromDomain).ToList();

        return Result.Success(response);
    }

    private static List<Road> SelectAvailableRoadPoints(
        Board board,
        PlayerColour playerColour)
    {
        var availableRoadPoints = new List<Road>();

        var roads = board.GetRoads();

        foreach (var road in roads)
        {
            if (board.CanPlaceRoadBetweenPoints(
                road.FirstPoint,
                road.SecondPoint,
                playerColour))
            {
                availableRoadPoints.Add(road);
            }
        }

        return availableRoadPoints;
    }
}
