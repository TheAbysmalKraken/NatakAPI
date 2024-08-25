using System.Net;

namespace Catan.Domain.Errors;

public static class GameStateManagerErrors
{
    public static Error InvalidAction => new(
        HttpStatusCode.BadRequest,
        "GameStateManager.InvalidAction",
        "Invalid action at current state.");
}
