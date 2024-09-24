using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.EmbargoPlayer;

public sealed record EmbargoPlayerCommand(
    string GameId,
    int PlayerColour,
    int PlayerColourToEmbargo)
    : ICommand;
