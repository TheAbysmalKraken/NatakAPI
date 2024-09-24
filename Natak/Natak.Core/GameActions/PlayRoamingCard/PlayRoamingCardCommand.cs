using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.PlayRoamingCard;

public sealed record PlayRoamingCardCommand(
    string GameId)
    : ICommand;
