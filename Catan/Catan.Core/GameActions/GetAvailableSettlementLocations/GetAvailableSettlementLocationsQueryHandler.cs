using Catan.Core.Abstractions;
using Catan.Core.Models;
using Catan.Core.Services;
using Catan.Domain;
using Catan.Domain.Enums;
using Catan.Domain.Errors;

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
            return Result.Failure<List<PointResponse>>(GameErrors.GameNotFound);
        }

        var playerColour = (PlayerColour)request.PlayerColour;
        if (game.GetPlayer(playerColour) is null)
        {
            return Result.Failure<List<PointResponse>>(PlayerErrors.NotFound);
        }

        var board = game.Board;

        var availableSettlementPoints = SelectAvailableSettlementPoints(
            board,
            game.IsSetup,
            playerColour);

        var response = availableSettlementPoints.Select(PointResponse.FromPoint).ToList();

        return Result.Success(response);
    }

    private static List<Point> SelectAvailableSettlementPoints(
        Board board,
        bool isSetup,
        PlayerColour playerColour)
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
                    isSetup);

                if (canPlaceHouseResult.IsSuccess)
                {
                    availableSettlementPoints.Add(point);
                }
            }
        }

        return availableSettlementPoints;
    }
}
