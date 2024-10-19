using System.Net;

namespace Natak.Domain;

public sealed record Error(HttpStatusCode StatusCode, string Type, string Message)
{
    public static Error None => new(HttpStatusCode.OK, "Error.None", "No error.");

    public override string ToString()
    {
        return $"[{Type}] {Message}";
    }
}
