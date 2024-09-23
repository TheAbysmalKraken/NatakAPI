using System.Text.Json.Serialization;
using Natak.Domain;

namespace Natak.Core.Models;

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

    [JsonPropertyName("cardsToDiscard")]
    public required int CardsToDiscard { get; init; }

    public static PlayerResponse FromDomain(Player player)
    {
        var resourceCards = player.ResourceCardManager.Cards;
        var playableDevelopmentCards = player.DevelopmentCardManager.Cards;
        var onHoldDevelopmentCards = player.DevelopmentCardManager.OnHoldCards;

        return new PlayerResponse
        {
            Colour = (int)player.Colour,
            KnightsPlayed = player.KnightsPlayed,
            VisibleVictoryPoints = player.ScoreManager.VisiblePoints,
            TotalResourceCards = resourceCards.Sum(kvp => kvp.Value),
            TotalDevelopmentCards = playableDevelopmentCards.Sum(kvp => kvp.Value) + onHoldDevelopmentCards.Sum(kvp => kvp.Value),
            HasLargestArmy = player.ScoreManager.HasLargestArmy,
            HasLongestRoad = player.ScoreManager.HasLongestRoad,
            RemainingSettlements = player.PieceManager.Settlements,
            RemainingCities = player.PieceManager.Cities,
            RemainingRoads = player.PieceManager.Roads,
            CardsToDiscard = player.CardsToDiscard
        };
    }
}
