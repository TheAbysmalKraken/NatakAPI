using System.Net;
using Catan.Core.Models;

namespace Catan.Core.GameActions.CreateGame;

public static class CreateGameErrors
{
    public static readonly Error InvalidPlayerCount = new(
        HttpStatusCode.BadRequest,
        "Catan.InvalidPlayerCount",
        "Player count must be between 3 and 4.");
}
