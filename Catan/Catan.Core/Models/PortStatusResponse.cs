using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.Application.Models;

public sealed class PortStatusResponse
{
    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    [JsonPropertyName("type")]
    public int Resource { get; set; }

    public static PortStatusResponse FromDomain(Port port)
    {
        return new PortStatusResponse
        {
            X = port.Point.X,
            Y = port.Point.Y,
            Resource = (int)port.Type
        };
    }
}
