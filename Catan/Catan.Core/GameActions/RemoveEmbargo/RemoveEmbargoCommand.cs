using Catan.Core.Abstractions;

namespace Catan.Core.GameActions.RemoveEmbargo;

public sealed record RemoveEmbargoCommand(
    string GameId,
    int PlayerColour,
    int PlayerColourToRemoveEmbargoOn)
    : ICommand;
