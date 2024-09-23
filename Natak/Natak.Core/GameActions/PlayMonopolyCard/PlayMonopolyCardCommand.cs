using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.PlayMonopolyCard;

public sealed record PlayMonopolyCardCommand(
    string GameId,
    int Resource)
    : ICommand;
