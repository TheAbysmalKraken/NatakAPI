using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.PlayGathererCard;

public sealed record PlayGathererCardCommand(
    string GameId,
    int Resource)
    : ICommand;
