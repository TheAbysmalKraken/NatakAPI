using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.StealResource;

public sealed record StealResourceCommand(
    string GameId,
    int VictimColour)
    : ICommand;
