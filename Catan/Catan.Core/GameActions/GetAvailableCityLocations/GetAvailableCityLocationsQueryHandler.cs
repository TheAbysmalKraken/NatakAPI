using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;

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
            return Result.Failure<List<PointResponse>>(Errors.GameNotFound);
        }

        var playerColour = (PlayerColour)request.PlayerColour;
        if (!game.ContainsPlayer(playerColour))
        {
            return Result.Failure<List<PointResponse>>(Errors.InvalidPlayerColour);
        }

        var board = game.Board;
        var buildings = board.GetHouses();

        var settlementPoints = SelectSettlementsOfColourPoints(
            buildings,
            playerColour);

        var response = settlementPoints.Select(PointResponse.FromPoint).ToList();

        return Result.Success(response);
    }

    private static List<Point> SelectSettlementsOfColourPoints(Building[,] buildings, PlayerColour playerColour)
    {
        var settlementPoints = new List<Point>();

        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                var building = buildings[x, y];

                if (building is not null
                    && building.Type == BuildingType.Settlement
                    && building.Colour == playerColour)
                {
                    settlementPoints.Add(new(x, y));
                }
            }
        }

        return settlementPoints;
    }
}
