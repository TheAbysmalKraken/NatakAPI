using Catan.Application.Models;

namespace Catan.Core.GameActions.GetAvailableSettlementLocations;

public sealed record GetAvailableSettlementLocationsQuery(
    string GameId,
    int PlayerColour)
    : IQuery<IList<PointResponse>>;
