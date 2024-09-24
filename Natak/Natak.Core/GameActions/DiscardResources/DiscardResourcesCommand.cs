using Natak.Core.Abstractions;
using Natak.Domain.Enums;

namespace Natak.Core.GameActions.DiscardResources;

public sealed record DiscardResourcesCommand(
    string GameId,
    int PlayerColour,
    Dictionary<ResourceType, int> Resources)
    : ICommand;
