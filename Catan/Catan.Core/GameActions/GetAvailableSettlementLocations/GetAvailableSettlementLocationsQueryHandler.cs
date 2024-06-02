using Catan.Core.Models;
using Catan.Core.Services;

namespace Catan.Core.GameActions.GetAvailableSettlementLocations;

internal sealed class GetAvailableSettlementLocationsQueryHandler(
    IActiveGameCache cache)
    : IQueryHandler<GetAvailableSettlementLocationsQuery, IList<PointResponse>>
{
    public async Task<Result<IList<PointResponse>>> Handle(
        GetAvailableSettlementLocationsQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
