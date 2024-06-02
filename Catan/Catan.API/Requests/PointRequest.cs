using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.API.Requests;

public sealed class PointRequest
{
    [JsonPropertyName("x")]
    public required int X { get; init; }

    [JsonPropertyName("y")]
    public required int Y { get; init; }

    public Point ToPoint() => new(X, Y);
}
