using System.Text.Json.Serialization;
using Catan.Domain;
using static Catan.Common.Enumerations;

namespace Catan.Application.Models;

public sealed class BoardStatusResponse
{
    [JsonPropertyName("hexes")]
    public List<HexStatusResponse> Hexes { get; set; } = [];

    [JsonPropertyName("roads")]
    public List<RoadStatusResponse> Roads { get; set; } = [];

    [JsonPropertyName("settlements")]
    public List<BuildingStatusResponse> Settlements { get; set; } = [];

    [JsonPropertyName("cities")]
    public List<BuildingStatusResponse> Cities { get; set; } = [];

    [JsonPropertyName("ports")]
    public List<PortStatusResponse> Ports { get; set; } = [];

    public static BoardStatusResponse FromDomain(Board board)
    {
        var boardStatusResponse = new BoardStatusResponse
        {
            Roads = board.GetRoads().Select(RoadStatusResponse.FromDomain).ToList(),
            Ports = board.GetPorts().Select(PortStatusResponse.FromDomain).ToList()
        };

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                var hex = board.GetTile(new Point(x, y));

                if (hex is not null)
                    boardStatusResponse.Hexes.Add(HexStatusResponse.FromDomain(hex, x, y));
            }
        }

        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                var house = board.GetHouse(new Point(x, y));

                if (house is not null)
                {
                    if (house.Type == BuildingType.Settlement)
                    {
                        boardStatusResponse.Settlements.Add(BuildingStatusResponse.FromDomain(house, x, y));
                    }
                    else if (house.Type == BuildingType.City)
                    {
                        boardStatusResponse.Cities.Add(BuildingStatusResponse.FromDomain(house, x, y));
                    }
                }
            }
        }

        return boardStatusResponse;
    }
}
