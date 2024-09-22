using Catan.Core.Abstractions;
using Catan.Domain;

namespace Catan.Core.GameActions.PlayRoadBuildingCard;

public sealed record PlayRoadBuildingCardCommand(
    string GameId)
    : ICommand;
