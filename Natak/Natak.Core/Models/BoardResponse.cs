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
        var boardStatusResponse = new BoardResponse
        {
            Hexes = [],
            Roads = board.GetRoads().Select(RoadResponse.FromDomain).ToList(),
            Villages = [],
            Towns = [],
            Ports = board.GetPorts().Select(PortResponse.FromDomain).ToList()
        };

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                var point = new Point(x, y);
                var hex = board.GetTile(point);

                if (hex is not null)
                    boardStatusResponse.Hexes.Add(HexResponse.FromDomain(hex, point));
            }
        }

        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                var point = new Point(x, y);
                var house = board.GetHouse(point);

                if (house is not null)
                {
                    if (house.Type == HouseType.Village)
                    {
                        boardStatusResponse.Villages.Add(BuildingResponse.FromDomain(house, point));
                    }
                    else if (house.Type == HouseType.Town)
                    {
                        boardStatusResponse.Towns.Add(BuildingResponse.FromDomain(house, point));
                    }
                }
            }
        }

        return boardStatusResponse;
    }
}
