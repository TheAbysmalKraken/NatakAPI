using Catan.Core.Abstractions;
using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.GetAvailableSettlementLocations;

internal sealed class GetAvailableSettlementLocationsQueryHandler(
    IActiveGameCache cache)
    : IQueryHandler<GetAvailableSettlementLocationsQuery, List<PointResponse>>
{
    public async Task<Result<List<PointResponse>>> Handle(
        GetAvailableSettlementLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var game = await cache.GetAsync(request.GameId, cancellationToken);

        if (game is null)
        {
            return Result.Failure<List<PointResponse>>(GeneralErrors.GameNotFound);
        }

        var playerColour = (PlayerColour)request.PlayerColour;
        if (!game.ContainsPlayer(playerColour))
        {
            return Result.Failure<List<PointResponse>>(GeneralErrors.InvalidPlayerColour);
        }

        var board = game.Board;

        var availableSettlementPoints = SelectAvailableSettlementPoints(
            board,
            playerColour,
            request.IsInitialPlacement);

        var response = availableSettlementPoints.Select(PointResponse.FromPoint).ToList();

        return Result.Success(response);
    }

    private static List<Point> SelectAvailableSettlementPoints(
        Board board,
        PlayerColour playerColour,
        bool isInitialPlacement)
    {
        var availableSettlementPoints = new List<Point>();

        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                var point = new Point(x, y);

                var canPlaceHouseResult = board.CanPlaceHouseAtPoint(
                    point,
                    playerColour,
                    isInitialPlacement);

                if (canPlaceHouseResult.IsSuccess)
                {
                    availableSettlementPoints.Add(point);
                }
            }
        }

        return availableSettlementPoints;
    }
}
