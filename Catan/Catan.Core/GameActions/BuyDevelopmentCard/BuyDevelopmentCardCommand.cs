using Catan.Core.Abstractions;

namespace Catan.Core.GameActions.BuyDevelopmentCard;

public sealed record BuyDevelopmentCardCommand(string GameId) : ICommand;
