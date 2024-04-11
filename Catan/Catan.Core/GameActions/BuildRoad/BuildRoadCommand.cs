using Catan.Domain;

namespace Catan.Core.GameActions.BuildRoad;

public sealed record BuildRoadCommand(
    string GameId,
    Point FirstPoint,
    Point SecondPoint) : ICommand;
