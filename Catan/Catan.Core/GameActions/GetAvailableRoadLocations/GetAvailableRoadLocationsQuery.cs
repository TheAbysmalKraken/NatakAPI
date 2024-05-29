using Catan.Application.Models;

namespace Catan.Core.GameActions.GetAvailableRoadLocations;

public sealed record GetAvailableRoadLocationsQuery(
    string GameId,
    int PlayerColour)
    : IQuery<IList<RoadPointsResponse>>;
