using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.Application.Models;

public sealed class BuildingStatusResponse
{
    [JsonPropertyName("playerColour")]
    public int PlayerColour { get; set; }

    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    public static BuildingStatusResponse FromDomain(CatanBuilding house, int x, int y)
    {
        return new BuildingStatusResponse
        {
            PlayerColour = (int)house.Colour,
            X = x,
            Y = y
        };
    }
}
