namespace Catan.Core.GameActions.RespondToTradeOffer;

public sealed record RespondToTradeOfferCommand(
    string GameId,
    int PlayerColour,
    bool Accept)
    : ICommand;
