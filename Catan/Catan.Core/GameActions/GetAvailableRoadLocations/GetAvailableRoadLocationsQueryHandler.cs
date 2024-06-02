using Catan.Core.Models;
using Catan.Core.Services;

namespace Catan.Core.GameActions.GetAvailableRoadLocations;

internal sealed class GetAvailableRoadLocationsQueryHandler(
    IActiveGameCache cache)
    : IQueryHandler<GetAvailableRoadLocationsQuery, IList<RoadResponse>>
{
    public async Task<Result<IList<RoadResponse>>> Handle(
        GetAvailableRoadLocationsQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
