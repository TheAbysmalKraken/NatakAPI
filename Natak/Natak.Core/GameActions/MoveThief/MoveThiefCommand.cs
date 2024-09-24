using Natak.Core.Abstractions;
using Natak.Domain;

namespace Natak.Core.GameActions.MoveThief;

public sealed record MoveThiefCommand(
    string GameId,
    Point MoveThiefTo)
    : ICommand;
