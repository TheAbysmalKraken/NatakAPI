namespace Catan.Core.GameActions.RollDice;

public sealed record RollDiceCommand(string GameId) : ICommand<RollDiceResponse>;
