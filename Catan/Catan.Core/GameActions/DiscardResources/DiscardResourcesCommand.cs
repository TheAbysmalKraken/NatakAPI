using Catan.Core.Abstractions;
using Catan.Domain.Enums;

namespace Catan.Core.GameActions.DiscardResources;

public sealed record DiscardResourcesCommand(
    string GameId,
    int PlayerColour,
    Dictionary<ResourceType, int> Resources)
    : ICommand;
