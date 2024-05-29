using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.Application.Models;

public sealed class RoadStatusResponse
{
    [JsonPropertyName("playerColour")]
    public int PlayerColour { get; set; }

    [JsonPropertyName("startX")]
    public int StartX { get; set; }

    [JsonPropertyName("startY")]
    public int StartY { get; set; }

    [JsonPropertyName("endX")]
    public int EndX { get; set; }

    [JsonPropertyName("endY")]
    public int EndY { get; set; }

    public static RoadStatusResponse FromDomain(Road road)
    {
        return new RoadStatusResponse
        {
            PlayerColour = (int)road.Colour,
            StartX = road.FirstPoint.X,
            StartY = road.FirstPoint.Y,
            EndX = road.SecondPoint.X,
            EndY = road.SecondPoint.Y
        };
    }
}
