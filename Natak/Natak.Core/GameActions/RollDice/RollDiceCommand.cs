using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.RollDice;

public sealed record RollDiceCommand(string GameId) : ICommand<RollDiceResponse>;
