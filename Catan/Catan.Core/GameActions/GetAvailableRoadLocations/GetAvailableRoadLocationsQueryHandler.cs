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

        var playerColour = (PlayerColour)request.PlayerColour;
        if (!game.ContainsPlayer(playerColour))
        {
            return Result.Failure<List<RoadResponse>>(PlayerErrors.InvalidPlayerColour);
        }

        var board = game.Board;

        var availableRoadPoints = SelectAvailableRoadPoints(
            board,
            game.CurrentState,
            playerColour);

        var response = availableRoadPoints.Select(RoadResponse.FromDomain).ToList();

        return Result.Success(response);
    }

    private static List<Road> SelectAvailableRoadPoints(
        Board board,
        GameState gameState,
        PlayerColour playerColour)
    {
        var availableRoadPoints = new List<Road>();
        bool isSetup = gameState == GameState.FirstRoad
            || gameState == GameState.SecondRoad;

        var roads = board.GetRoads();

        foreach (var road in roads)
        {
            var canPlaceRoadResult = isSetup
                ? board.CanPlaceSetupRoadBetweenPoints(
                    road.FirstPoint,
                    road.SecondPoint,
                    playerColour)
                : board.CanPlaceRoadBetweenPoints(
                    road.FirstPoint,
                    road.SecondPoint,
                    playerColour);

            if (canPlaceRoadResult.IsSuccess)
            {
                availableRoadPoints.Add(road);
            }
        }

        return availableRoadPoints;
    }
}
