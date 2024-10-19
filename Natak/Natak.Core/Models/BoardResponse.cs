using System.Text.Json.Serialization;
using Natak.Domain;
using Natak.Domain.Enums;

namespace Natak.Core.Models;

public sealed class BoardResponse
{
    [JsonPropertyName("hexes")]
    public required List<HexResponse> Hexes { get; init; }

    [JsonPropertyName("roads")]
    public required List<RoadResponse> Roads { get; init; }

    [JsonPropertyName("villages")]
    public required List<BuildingResponse> Villages { get; init; }

    [JsonPropertyName("towns")]
    public required List<BuildingResponse> Towns { get; init; }

    [JsonPropertyName("ports")]
    public required List<PortResponse> Ports { get; init; }

    public static BoardResponse FromDomain(Board board)
    {
        var houses = board.GetHouses();
        
        return new BoardResponse
        {
            Hexes = board.GetTiles().Select(HexResponse.FromDomain).ToList(),
            Roads = board.GetRoads().Select(RoadResponse.FromDomain).ToList(),
            Villages = houses.Where(h => h.Type == HouseType.Village).Select(BuildingResponse.FromDomain).ToList(),
            Towns = houses.Where(h => h.Type == HouseType.Town).Select(BuildingResponse.FromDomain).ToList(),
            Ports = board.GetPorts().Select(PortResponse.FromDomain).ToList()
        };
    }
}
