using Natak.Core.Abstractions;
using Natak.Core.Models;

namespace Natak.Core.GameActions.GetAvailableVillageLocations;

public sealed record GetAvailableVillageLocationsQuery(
    string GameId)
    : IQuery<List<PointResponse>>;
