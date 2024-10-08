using System.Net;

namespace Natak.Domain.Errors;

public static class GameStateManagerErrors
{
    public static Error InvalidAction => new(
        HttpStatusCode.BadRequest,
        "StateManager.InvalidAction",
        "Invalid action at current state.");
}
