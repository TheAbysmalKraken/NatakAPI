using Catan.Application.Models;

namespace Catan.Core.GameActions.GetAvailableCityLocations;

public sealed record GetAvailableCityLocationsQuery(
    string GameId,
    int PlayerColour)
    : IQuery<IList<PointResponse>>;
