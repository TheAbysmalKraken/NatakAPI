using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.API.Controllers.Models;

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

    public static RoadStatusResponse FromDomain(CatanRoad road)
    {
        return new RoadStatusResponse
        {
            PlayerColour = (int)road.Colour,
            StartX = road.FirstCornerCoordinates.X,
            StartY = road.FirstCornerCoordinates.Y,
            EndX = road.SecondCornerCoordinates.X,
            EndY = road.SecondCornerCoordinates.Y
        };
    }
}
