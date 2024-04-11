using Catan.Application.Models;

namespace Catan.Core.GameActions.GetGame;

public sealed record GetGameQuery(
    string GameId,
    int PlayerColour) : IQuery<PlayerSpecificGameStatusResponse>;
