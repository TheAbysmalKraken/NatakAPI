using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.PlayWealthCard;

public sealed record PlayWealthCardCommand(
    string GameId,
    int FirstResource,
    int SecondResource)
    : ICommand;
