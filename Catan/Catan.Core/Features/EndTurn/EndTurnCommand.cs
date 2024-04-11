namespace Catan.Core.Features.EndTurn;

public sealed record EndTurnCommand(string GameId) : ICommand;
