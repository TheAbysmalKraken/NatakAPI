using Catan.Application.Models;

namespace Catan.Core.Features.GetGame;

public sealed record GetGameQuery(
    string GameId,
    int PlayerColour) : IQuery<PlayerSpecificGameStatusResponse>;
