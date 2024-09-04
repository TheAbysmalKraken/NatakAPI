using System.Net;

namespace Catan.Domain.Errors;

public static class ItemManagerErrors
{
    public static Error NotEnough => new(
        HttpStatusCode.BadRequest,
        "ItemManager.NotEnough",
        "Not enough items to remove.");

    public static Error NotFound => new(
        HttpStatusCode.NotFound,
        "ItemManager.NotFound",
        "Item not found.");
}
