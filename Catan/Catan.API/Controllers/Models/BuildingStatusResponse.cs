using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.API.Controllers.Models;

public sealed class BuildingStatusResponse
{
    [JsonPropertyName("playerId")]
    public int PlayerId { get; set; }

    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    public static BuildingStatusResponse FromDomain(CatanBuilding house, int x, int y)
    {
        return new BuildingStatusResponse
        {
            PlayerId = (int)house.Colour,
            X = x,
            Y = y
        };
    }
}
