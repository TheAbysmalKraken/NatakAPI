using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.Core.Models;

public sealed class PointResponse
{
    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    public static PointResponse FromPoint(Point point)
    {
        return new()
        {
            X = point.X,
            Y = point.Y
        };
    }
}
