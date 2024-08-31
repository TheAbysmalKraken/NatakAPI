using System.Net;

namespace Catan.Domain.Errors;

public static class CardManagerErrors
{
    public static Error NotEnough => new(
        HttpStatusCode.BadRequest,
        "CardManager.NotEnough",
        "Not enough cards to remove.");

    public static Error NotFound => new(
        HttpStatusCode.NotFound,
        "CardManager.NotFound",
        "Card not found.");
}
