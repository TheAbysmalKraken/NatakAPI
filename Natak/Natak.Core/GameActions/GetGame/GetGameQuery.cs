using Natak.Core.Abstractions;
using Natak.Core.Models;

namespace Natak.Core.GameActions.GetGame;

public sealed record GetGameQuery(
    string GameId,
    int PlayerColour) : IQuery<GameResponse>;
