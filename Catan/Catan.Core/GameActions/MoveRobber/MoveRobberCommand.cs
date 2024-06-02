using Catan.Domain;

namespace Catan.Core.GameActions.MoveRobber;

public sealed record MoveRobberCommand(
    string GameId,
    Point MoveRobberTo)
    : ICommand;
