using Natak.Core.Abstractions;
using Natak.Domain;

namespace Natak.Core.GameActions.MoveRobber;

public sealed record MoveRobberCommand(
    string GameId,
    Point MoveRobberTo)
    : ICommand;
