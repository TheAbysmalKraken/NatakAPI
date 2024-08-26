using Catan.Core.Abstractions;
using Catan.Core.Models;

namespace Catan.Core.GameActions.GetGame;

public sealed record GetGameQuery(
    string GameId,
    int PlayerColour) : IQuery<GameResponse>;
