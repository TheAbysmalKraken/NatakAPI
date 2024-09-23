using Natak.Core.Abstractions;
using Natak.Domain;

namespace Natak.Core.GameActions.BuildSettlement;

public sealed record BuildSettlementCommand(string GameId, Point BuildPoint) : ICommand;
