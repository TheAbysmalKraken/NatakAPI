using Natak.Core.Abstractions;
using Natak.Domain;

namespace Natak.Core.GameActions.PlayKnightCard;

public sealed record PlayKnightCardCommand(string GameId) : ICommand;
