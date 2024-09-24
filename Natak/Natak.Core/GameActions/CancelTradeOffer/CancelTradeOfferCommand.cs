using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.CancelTradeOffer;

public sealed record CancelTradeOfferCommand(string GameId) : ICommand;
