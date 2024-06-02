using Catan.Core.Models;

namespace Catan.Core.GameActions.GetAvailableRoadLocations;

public sealed record GetAvailableRoadLocationsQuery(
    string GameId,
    int PlayerColour)
    : IQuery<IList<RoadResponse>>;
