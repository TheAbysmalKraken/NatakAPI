namespace Catan.Core.GameActions.PlayYearOfPlentyCard;

public sealed record PlayYearOfPlentyCardCommand(
    string GameId,
    int FirstResource,
    int SecondResource)
    : ICommand;
