using Catan.Core.Abstractions;

namespace Catan.Core.GameActions.EmbargoPlayer;

public sealed record EmbargoPlayerCommand(
    string GameId,
    int PlayerColour,
    int PlayerColourToEmbargo)
    : ICommand;
