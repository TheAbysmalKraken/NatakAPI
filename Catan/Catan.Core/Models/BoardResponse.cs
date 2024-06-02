using System.Text.Json.Serialization;
using Catan.Domain;
using Catan.Domain.Enums;

namespace Catan.Core.Models;

public sealed class BoardResponse
{
    [JsonPropertyName("hexes")]
    public required List<HexResponse> Hexes { get; init; }

    [JsonPropertyName("roads")]
    public required List<RoadResponse> Roads { get; init; }

    [JsonPropertyName("settlements")]
    public required List<BuildingResponse> Settlements { get; init; }

    [JsonPropertyName("cities")]
    public required List<BuildingResponse> Cities { get; init; }

    [JsonPropertyName("ports")]
    public required List<PortResponse> Ports { get; init; }

    public static BoardResponse FromDomain(Board board)
    {
        var boardStatusResponse = new BoardResponse
        {
            Hexes = [],
            Roads = board.GetRoads().Select(RoadResponse.FromDomain).ToList(),
            Settlements = [],
            Cities = [],
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
                    if (house.Type == BuildingType.Settlement)
                    {
                        boardStatusResponse.Settlements.Add(BuildingResponse.FromDomain(house, point));
                    }
                    else if (house.Type == BuildingType.City)
                    {
                        boardStatusResponse.Cities.Add(BuildingResponse.FromDomain(house, point));
                    }
                }
            }
        }

        return boardStatusResponse;
    }
}
