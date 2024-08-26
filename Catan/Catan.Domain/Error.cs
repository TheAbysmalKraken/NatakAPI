using System.Net;

namespace Catan.Domain;

public sealed record Error(HttpStatusCode StatusCode, string Type, string Message)
{
    public static Error None => new(HttpStatusCode.OK, "Error.None", "No error.");
}
