using Catan.Domain.Enums;

namespace Catan.Core.GameActions.MakeTradeOffer;

public sealed record MakeTradeOfferCommand(
    string GameId,
    Dictionary<ResourceType, int> Offer,
    Dictionary<ResourceType, int> Request)
    : ICommand;
