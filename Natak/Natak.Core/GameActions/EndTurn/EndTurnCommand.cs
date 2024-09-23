using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.EndTurn;

public sealed record EndTurnCommand(string GameId) : ICommand;
