using Natak.Core.Abstractions;
using Natak.Core.Models;

namespace Natak.Core.GameActions.GetAvailableTownLocations;

public sealed record GetAvailableTownLocationsQuery(
    string GameId)
    : IQuery<List<PointResponse>>;
