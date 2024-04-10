using Catan.Application.Models;
using MediatR;

namespace Catan.Core.Features.GetGame;

public sealed record GetGameQuery(
    string GameId,
    int PlayerColour) : IRequest<Result<PlayerSpecificGameStatusResponse>>;
