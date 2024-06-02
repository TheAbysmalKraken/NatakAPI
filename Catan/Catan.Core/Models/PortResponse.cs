using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.Core.Models;

public sealed class PortResponse
{
    [JsonPropertyName("point")]
    public required PointResponse Point { get; init; }

    [JsonPropertyName("type")]
    public required int Resource { get; init; }

    public static PortResponse FromDomain(Port port)
    {
        return new PortResponse
        {
            Point = PointResponse.FromPoint(port.Point),
            Resource = (int)port.Type
        };
    }
}
