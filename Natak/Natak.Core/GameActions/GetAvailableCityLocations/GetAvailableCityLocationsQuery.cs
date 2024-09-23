using Natak.Core.Abstractions;
using Natak.Core.Models;

namespace Natak.Core.GameActions.GetAvailableCityLocations;

public sealed record GetAvailableCityLocationsQuery(
    string GameId)
    : IQuery<List<PointResponse>>;
