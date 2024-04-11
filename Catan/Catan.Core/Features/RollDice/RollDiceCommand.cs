namespace Catan.Core.Features.RollDice;

public sealed record RollDiceCommand(string GameId) : ICommand<RollDiceResponse>;
