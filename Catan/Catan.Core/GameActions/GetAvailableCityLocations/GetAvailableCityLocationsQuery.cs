using Catan.Core.Abstractions;
using Catan.Core.Models;

namespace Catan.Core.GameActions.GetAvailableCityLocations;

public sealed record GetAvailableCityLocationsQuery(
    string GameId)
    : IQuery<List<PointResponse>>;
