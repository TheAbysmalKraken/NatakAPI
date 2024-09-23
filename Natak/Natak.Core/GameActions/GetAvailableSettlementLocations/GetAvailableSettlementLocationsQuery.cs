using Natak.Core.Abstractions;
using Natak.Core.Models;

namespace Natak.Core.GameActions.GetAvailableSettlementLocations;

public sealed record GetAvailableSettlementLocationsQuery(
    string GameId)
    : IQuery<List<PointResponse>>;
