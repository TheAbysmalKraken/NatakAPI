using Catan.Domain;

namespace Catan.Core.GameActions.BuildSettlement;

public sealed record BuildSettlementCommand(string GameId, Point BuildPoint) : ICommand;
