using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.API;

public class HexStatusResponse
{
    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    [JsonPropertyName("resource")]
    public int Resource { get; set; }

    [JsonPropertyName("rollNumber")]
    public int RollNumber { get; set; }

    public static HexStatusResponse FromDomain(CatanTile hex, int x, int y)
    {
        return new HexStatusResponse
        {
            X = x,
            Y = y,
            Resource = (int)hex.Type,
            RollNumber = hex.ActivationNumber
        };
    }
}
