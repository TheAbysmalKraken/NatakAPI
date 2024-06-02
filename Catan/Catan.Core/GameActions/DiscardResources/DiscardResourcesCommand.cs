namespace Catan.Core.GameActions.DiscardResources;

public sealed record DiscardResourcesCommand(
    string GameId,
    int PlayerColour,
    Dictionary<int, int> Resources)
    : ICommand;
