using Natak.Core.Abstractions;
using Natak.Domain.Enums;

namespace Natak.Core.GameActions.MakeTradeOffer;

public sealed record MakeTradeOfferCommand(
    string GameId,
    Dictionary<ResourceType, int> Offer,
    Dictionary<ResourceType, int> Request)
    : ICommand;
