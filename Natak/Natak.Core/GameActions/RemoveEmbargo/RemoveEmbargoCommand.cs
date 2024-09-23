using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.RemoveEmbargo;

public sealed record RemoveEmbargoCommand(
    string GameId,
    int PlayerColour,
    int PlayerColourToRemoveEmbargoOn)
    : ICommand;
