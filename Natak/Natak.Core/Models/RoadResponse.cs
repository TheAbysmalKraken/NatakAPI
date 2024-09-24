using System.Text.Json.Serialization;
using Natak.Domain;

namespace Natak.Core.Models;

public sealed class RoadResponse
{
    [JsonPropertyName("playerColour")]
    public required int PlayerColour { get; init; }

    [JsonPropertyName("firstPoint")]
    public required PointResponse FirstPoint { get; init; }

    [JsonPropertyName("secondPoint")]
    public required PointResponse SecondPoint { get; init; }

    public static RoadResponse FromDomain(Road road)
    {
        return new RoadResponse
        {
            PlayerColour = (int)road.Colour,
            FirstPoint = PointResponse.FromPoint(road.FirstPoint),
            SecondPoint = PointResponse.FromPoint(road.SecondPoint)
        };
    }
}
