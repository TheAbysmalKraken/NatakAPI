namespace Catan.Core.GameActions.StealResource;

public sealed record StealResourceCommand(
    string GameId,
    int VictimColour)
    : ICommand;
