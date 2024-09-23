using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.BuyDevelopmentCard;

public sealed record BuyDevelopmentCardCommand(string GameId) : ICommand;
