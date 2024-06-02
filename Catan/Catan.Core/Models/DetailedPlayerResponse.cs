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
        var resourceCards = player.GetResourceCards();
        var playableDevelopmentCards = player.GetPlayableDevelopmentCards();
        var onHoldDevelopmentCards = player.GetDevelopmentCardsOnHold();

        return new()
        {
            VictoryPoints = player.VictoryPoints,
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
            EmbargoedPlayerColours = baseResponse.EmbargoedPlayerColours
        };
    }
}
