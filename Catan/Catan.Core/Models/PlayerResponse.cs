using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.Core.Models;

public class PlayerResponse
{
    [JsonPropertyName("colour")]
    public required int Colour { get; init; }

    [JsonPropertyName("knightsPlayed")]
    public required int KnightsPlayed { get; init; }

    [JsonPropertyName("visibleVictoryPoints")]
    public required int VisibleVictoryPoints { get; init; }

    [JsonPropertyName("totalResourceCards")]
    public required int TotalResourceCards { get; init; }

    [JsonPropertyName("totalDevelopmentCards")]
    public required int TotalDevelopmentCards { get; init; }

    [JsonPropertyName("hasLargestArmy")]
    public required bool HasLargestArmy { get; init; }

    [JsonPropertyName("hasLongestRoad")]
    public required bool HasLongestRoad { get; init; }

    [JsonPropertyName("remainingSettlements")]
    public required int RemainingSettlements { get; init; }

    [JsonPropertyName("remainingCities")]
    public required int RemainingCities { get; init; }

    [JsonPropertyName("remainingRoads")]
    public required int RemainingRoads { get; init; }

    [JsonPropertyName("embargoedPlayerColours")]
    public required List<int> EmbargoedPlayerColours { get; init; }

    public static PlayerResponse FromDomain(Player player)
    {
        var resourceCards = player.GetResourceCards();
        var playableDevelopmentCards = player.GetPlayableDevelopmentCards();
        var onHoldDevelopmentCards = player.GetDevelopmentCardsOnHold();

        return new PlayerResponse
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
    }
}
