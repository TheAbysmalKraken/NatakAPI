using Natak.Core.Abstractions;
using Natak.Domain;

namespace Natak.Core.GameActions.PlayRoadBuildingCard;

public sealed record PlayRoadBuildingCardCommand(
    string GameId)
    : ICommand;
