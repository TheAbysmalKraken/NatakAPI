using Natak.Core.Abstractions;
using Natak.Core.Models;
using Natak.Core.Services;
using Natak.Domain;
using Natak.Domain.Enums;
using Natak.Domain.Errors;

namespace Natak.Core.GameActions.GetAvailableSettlementLocations;

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

        var result = game.GetAvailableSettlementLocations();

        if (result.IsFailure)
        {
            return Result.Failure<List<PointResponse>>(result.Error);
        }

        var availableSettlementPoints = result.Value;

        var response = availableSettlementPoints.Select(PointResponse.FromPoint).ToList();

        return Result.Success(response);
    }
}
