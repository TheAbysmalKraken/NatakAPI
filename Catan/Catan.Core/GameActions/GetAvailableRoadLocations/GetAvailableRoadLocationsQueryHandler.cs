using Catan.Application.Models;
using Catan.Core.Services;

namespace Catan.Core.GameActions.GetAvailableRoadLocations;

internal sealed class GetAvailableRoadLocationsQueryHandler(
    IActiveGameCache cache)
    : IQueryHandler<GetAvailableRoadLocationsQuery, IList<RoadPointsResponse>>
{
    public async Task<Result<IList<RoadPointsResponse>>> Handle(
        GetAvailableRoadLocationsQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
