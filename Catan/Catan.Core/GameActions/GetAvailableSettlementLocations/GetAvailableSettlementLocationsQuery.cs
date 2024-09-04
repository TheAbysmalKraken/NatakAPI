using Catan.Core.Abstractions;
using Catan.Core.Models;

namespace Catan.Core.GameActions.GetAvailableSettlementLocations;

public sealed record GetAvailableSettlementLocationsQuery(
    string GameId,
    int PlayerColour)
    : IQuery<List<PointResponse>>;
