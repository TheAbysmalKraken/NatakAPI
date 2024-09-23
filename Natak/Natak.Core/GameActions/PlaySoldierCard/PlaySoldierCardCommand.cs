using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.PlaySoldierCard;

public sealed record PlaySoldierCardCommand(string GameId) : ICommand;
