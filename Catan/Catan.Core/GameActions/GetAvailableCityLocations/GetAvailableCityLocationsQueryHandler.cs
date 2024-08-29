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

        var playerColour = (PlayerColour)request.PlayerColour;
        if (!game.ContainsPlayer(playerColour))
        {
            return Result.Failure<List<PointResponse>>(PlayerErrors.InvalidPlayerColour);
        }

        var board = game.Board;

        var availableCityPoints = SelectCityPoints(
            board,
            playerColour);

        var response = availableCityPoints.Select(PointResponse.FromPoint).ToList();

        return Result.Success(response);
    }

    private static List<Point> SelectCityPoints(
        Board board,
        PlayerColour playerColour)
    {
        var availableCityPoints = new List<Point>();

        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                var point = new Point(x, y);

                var canUpgradeResult = board.CanUpgradeHouseAtPoint(point, playerColour);

                if (canUpgradeResult.IsSuccess)
                {
                    availableCityPoints.Add(point);
                }
            }
        }

        return availableCityPoints;
    }
}
