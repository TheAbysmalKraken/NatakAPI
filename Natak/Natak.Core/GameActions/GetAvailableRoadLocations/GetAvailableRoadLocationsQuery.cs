using Natak.Core.Abstractions;
using Natak.Core.Models;

namespace Natak.Core.GameActions.GetAvailableRoadLocations;

public sealed record GetAvailableRoadLocationsQuery(
    string GameId)
    : IQuery<List<RoadResponse>>;
