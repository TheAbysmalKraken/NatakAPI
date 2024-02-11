using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.Application.Models;

public sealed class PlayerStatusResponse
{
    [JsonPropertyName("colour")]
    public int Colour { get; set; }

    [JsonPropertyName("knightsPlayed")]
    public int KnightsPlayed { get; set; }

    [JsonPropertyName("victoryPoints")]
    public int? VictoryPoints { get; set; }

    [JsonPropertyName("visibleVictoryPoints")]
    public int VisibleVictoryPoints { get; set; }

    [JsonPropertyName("resourceCards")]
    public Dictionary<int, int>? ResourceCards { get; set; }

    [JsonPropertyName("totalResourceCards")]
    public int TotalResourceCards { get; set; }

    [JsonPropertyName("playableDevelopmentCards")]
    public Dictionary<int, int>? PlayableDevelopmentCards { get; set; }

    [JsonPropertyName("onHoldDevelopmentCards")]
    public Dictionary<int, int>? OnHoldDevelopmentCards { get; set; }

    [JsonPropertyName("totalDevelopmentCards")]
    public int TotalDevelopmentCards { get; set; }

    [JsonPropertyName("hasLargestArmy")]
    public bool HasLargestArmy { get; set; }

    [JsonPropertyName("hasLongestRoad")]
    public bool HasLongestRoad { get; set; }

    [JsonPropertyName("remainingSettlements")]
    public int RemainingSettlements { get; set; }

    [JsonPropertyName("remainingCities")]
    public int RemainingCities { get; set; }

    [JsonPropertyName("remainingRoads")]
    public int RemainingRoads { get; set; }

    [JsonPropertyName("embargoedPlayerColours")]
    public List<int> EmbargoedPlayerColours { get; set; } = [];

    public static PlayerStatusResponse FromDomain(CatanPlayer player, bool hideProperties = false)
    {
        var resourceCards = player.GetResourceCards();
        var playableDevelopmentCards = player.GetPlayableDevelopmentCards();
        var onHoldDevelopmentCards = player.GetDevelopmentCardsOnHold();

        var response = new PlayerStatusResponse
        {
            Colour = (int)player.Colour,
            KnightsPlayed = player.KnightsPlayed,
            VisibleVictoryPoints = player.VictoryPoints - player.VictoryPointCards,
            TotalResourceCards = resourceCards.Sum(kvp => kvp.Value),
            TotalDevelopmentCards = playableDevelopmentCards.Sum(kvp => kvp.Value) + onHoldDevelopmentCards.Sum(kvp => kvp.Value),
            HasLargestArmy = player.HasLargestArmy,
            HasLongestRoad = player.HasLongestRoad,
            RemainingSettlements = player.RemainingSettlements,
            RemainingCities = player.RemainingCities,
            RemainingRoads = player.RemainingRoads,
            EmbargoedPlayerColours = player.GetEmbargoedPlayers().Select(p => (int)p).ToList()
        };

        if (!hideProperties)
        {
            response.VictoryPoints = player.VictoryPoints;
            response.ResourceCards = resourceCards.Select(kvp => new KeyValuePair<int, int>((int)kvp.Key, kvp.Value)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            response.PlayableDevelopmentCards = playableDevelopmentCards.Select(kvp => new KeyValuePair<int, int>((int)kvp.Key, kvp.Value)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            response.OnHoldDevelopmentCards = onHoldDevelopmentCards.Select(kvp => new KeyValuePair<int, int>((int)kvp.Key, kvp.Value)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        return response;
    }
}
