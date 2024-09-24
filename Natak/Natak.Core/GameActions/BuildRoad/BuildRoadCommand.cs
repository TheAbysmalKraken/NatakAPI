using Natak.Core.Abstractions;
using Natak.Domain;

namespace Natak.Core.GameActions.BuildRoad;

public sealed record BuildRoadCommand(
    string GameId,
    Point FirstPoint,
    Point SecondPoint) : ICommand;
