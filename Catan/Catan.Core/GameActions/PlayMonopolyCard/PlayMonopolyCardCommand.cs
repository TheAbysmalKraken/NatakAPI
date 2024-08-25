using Catan.Core.Abstractions;

namespace Catan.Core.GameActions.PlayMonopolyCard;

public sealed record PlayMonopolyCardCommand(
    string GameId,
    int Resource)
    : ICommand;
