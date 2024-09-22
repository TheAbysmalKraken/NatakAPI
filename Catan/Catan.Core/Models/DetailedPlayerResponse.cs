using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.Core.Models;

public sealed class DetailedPlayerResponse : PlayerResponse
{
    [JsonPropertyName("victoryPoints")]
    public required int VictoryPoints { get; init; }

    [JsonPropertyName("resourceCards")]
    public required Dictionary<int, int> ResourceCards { get; init; }

    [JsonPropertyName("playableDevelopmentCards")]
    public required Dictionary<int, int> PlayableDevelopmentCards { get; init; }

    [JsonPropertyName("onHoldDevelopmentCards")]
    public required Dictionary<int, int> OnHoldDevelopmentCards { get; init; }

    new public static DetailedPlayerResponse FromDomain(Player player)
    {
        var baseResponse = PlayerResponse.FromDomain(player);
        var resourceCards = player.ResourceCardManager.Cards;
        var playableDevelopmentCards = player.DevelopmentCardManager.Cards;
        var onHoldDevelopmentCards = player.DevelopmentCardManager.OnHoldCards;

        return new()
        {
            VictoryPoints = player.ScoreManager.TotalPoints,
            ResourceCards = resourceCards
                .Select(kvp => new KeyValuePair<int, int>((int)kvp.Key, kvp.Value))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            PlayableDevelopmentCards = playableDevelopmentCards
                .Select(kvp => new KeyValuePair<int, int>((int)kvp.Key, kvp.Value))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            OnHoldDevelopmentCards = onHoldDevelopmentCards
                .Select(kvp => new KeyValuePair<int, int>((int)kvp.Key, kvp.Value))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            Colour = baseResponse.Colour,
            KnightsPlayed = baseResponse.KnightsPlayed,
            VisibleVictoryPoints = baseResponse.VisibleVictoryPoints,
            TotalResourceCards = baseResponse.TotalResourceCards,
            TotalDevelopmentCards = baseResponse.TotalDevelopmentCards,
            HasLargestArmy = baseResponse.HasLargestArmy,
            HasLongestRoad = baseResponse.HasLongestRoad,
            RemainingCities = baseResponse.RemainingCities,
            RemainingRoads = baseResponse.RemainingRoads,
            RemainingSettlements = baseResponse.RemainingSettlements,
            CardsToDiscard = baseResponse.CardsToDiscard
        };
    }
}
