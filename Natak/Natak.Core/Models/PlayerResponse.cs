using System.Text.Json.Serialization;
using Natak.Domain;

namespace Natak.Core.Models;

public class PlayerResponse
{
    [JsonPropertyName("colour")]
    public required int Colour { get; init; }

    [JsonPropertyName("soldiersPlayed")]
    public required int SoldiersPlayed { get; init; }

    [JsonPropertyName("visibleVictoryPoints")]
    public required int VisibleVictoryPoints { get; init; }

    [JsonPropertyName("totalResourceCards")]
    public required int TotalResourceCards { get; init; }

    [JsonPropertyName("totalGrowthCards")]
    public required int TotalGrowthCards { get; init; }

    [JsonPropertyName("hasLargestArmy")]
    public required bool HasLargestArmy { get; init; }

    [JsonPropertyName("hasLongestRoad")]
    public required bool HasLongestRoad { get; init; }

    [JsonPropertyName("remainingVillages")]
    public required int RemainingVillages { get; init; }

    [JsonPropertyName("remainingCities")]
    public required int RemainingCities { get; init; }

    [JsonPropertyName("remainingRoads")]
    public required int RemainingRoads { get; init; }

    [JsonPropertyName("cardsToDiscard")]
    public required int CardsToDiscard { get; init; }

    public static PlayerResponse FromDomain(Player player)
    {
        var resourceCards = player.ResourceCardManager.Cards;
        var playableGrowthCards = player.GrowthCardManager.Cards;
        var onHoldGrowthCards = player.GrowthCardManager.OnHoldCards;

        return new PlayerResponse
        {
            Colour = (int)player.Colour,
            SoldiersPlayed = player.SoldiersPlayed,
            VisibleVictoryPoints = player.ScoreManager.VisiblePoints,
            TotalResourceCards = resourceCards.Sum(kvp => kvp.Value),
            TotalGrowthCards = playableGrowthCards.Sum(kvp => kvp.Value) + onHoldGrowthCards.Sum(kvp => kvp.Value),
            HasLargestArmy = player.ScoreManager.HasLargestArmy,
            HasLongestRoad = player.ScoreManager.HasLongestRoad,
            RemainingVillages = player.PieceManager.Villages,
            RemainingCities = player.PieceManager.Cities,
            RemainingRoads = player.PieceManager.Roads,
            CardsToDiscard = player.CardsToDiscard
        };
    }
}
