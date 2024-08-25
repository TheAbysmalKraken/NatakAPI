using Catan.Core.Abstractions;

namespace Catan.Core.GameActions.EndTurn;

public sealed record EndTurnCommand(string GameId) : ICommand;
