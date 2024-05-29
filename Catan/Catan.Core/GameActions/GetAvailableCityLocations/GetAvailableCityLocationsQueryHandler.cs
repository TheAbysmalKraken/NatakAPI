using Catan.Application.Models;
using Catan.Core.Services;

namespace Catan.Core.GameActions.GetAvailableCityLocations;

internal sealed class GetAvailableCityLocationsQueryHandler(
    IActiveGameCache cache)
    : IQueryHandler<GetAvailableCityLocationsQuery, IList<PointResponse>>
{
    public async Task<Result<IList<PointResponse>>> Handle(
        GetAvailableCityLocationsQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
