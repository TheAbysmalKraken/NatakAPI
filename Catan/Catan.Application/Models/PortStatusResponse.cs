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

    public static PortStatusResponse FromDomain(CatanPort port)
    {
        return new PortStatusResponse
        {
            X = port.Coordinates.X,
            Y = port.Coordinates.Y,
            Resource = (int)port.Type
        };
    }
}
