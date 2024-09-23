using Natak.Core.Abstractions;

namespace Natak.Core.GameActions.BuyGrowthCard;

public sealed record BuyGrowthCardCommand(string GameId) : ICommand;
