using Catan.Core.Abstractions;
using Catan.Domain;

namespace Catan.Core.GameActions.PlayKnightCard;

public sealed record PlayKnightCardCommand(
    string GameId,
    Point MoveRobberTo,
    int PlayerColourToStealFrom)
    : ICommand;
