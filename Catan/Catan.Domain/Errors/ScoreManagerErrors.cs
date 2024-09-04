using System.Net;

namespace Catan.Domain.Errors;

public static class ScoreManagerErrors
{
    public static Error Negative => new(
        HttpStatusCode.InternalServerError,
        "ScoreManager.Negative",
        "Score cannot be negative.");
}
