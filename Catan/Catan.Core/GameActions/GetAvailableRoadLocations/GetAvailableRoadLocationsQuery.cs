using Catan.Core.Abstractions;
using Catan.Core.Models;

namespace Catan.Core.GameActions.GetAvailableRoadLocations;

public sealed record GetAvailableRoadLocationsQuery(
    string GameId)
    : IQuery<List<RoadResponse>>;
