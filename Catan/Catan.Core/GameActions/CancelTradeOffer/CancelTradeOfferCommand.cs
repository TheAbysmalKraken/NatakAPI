using Catan.Core.Abstractions;

namespace Catan.Core.GameActions.CancelTradeOffer;

public sealed record CancelTradeOfferCommand(string GameId) : ICommand;
