using System.Text.Json.Serialization;
using Natak.Domain;

namespace Natak.Core.Models;

public sealed class DetailedPlayerResponse : PlayerResponse
{
    [JsonPropertyName("victoryPoints")]
    public required int VictoryPoints { get; init; }

    [JsonPropertyName("resourceCards")]
    public required Dictionary<int, int> ResourceCards { get; init; }

    [JsonPropertyName("playableGrowthCards")]
    public required Dictionary<int, int> PlayableGrowthCards { get; init; }

    [JsonPropertyName("onHoldGrowthCards")]
    public required Dictionary<int, int> OnHoldGrowthCards { get; init; }

    new public static DetailedPlayerResponse FromDomain(Player player)
    {
        var baseResponse = PlayerResponse.FromDomain(player);
        var resourceCards = player.ResourceCardManager.Cards;
        var playableGrowthCards = player.GrowthCardManager.Cards;
        var onHoldGrowthCards = player.GrowthCardManager.OnHoldCards;

        return new()
        {
            VictoryPoints = player.ScoreManager.TotalPoints,
            ResourceCards = resourceCards
                .Select(kvp => new KeyValuePair<int, int>((int)kvp.Key, kvp.Value))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            PlayableGrowthCards = playableGrowthCards
                .Select(kvp => new KeyValuePair<int, int>((int)kvp.Key, kvp.Value))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            OnHoldGrowthCards = onHoldGrowthCards
                .Select(kvp => new KeyValuePair<int, int>((int)kvp.Key, kvp.Value))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            Colour = baseResponse.Colour,
            SoldiersPlayed = baseResponse.SoldiersPlayed,
            VisibleVictoryPoints = baseResponse.VisibleVictoryPoints,
            TotalResourceCards = baseResponse.TotalResourceCards,
            TotalGrowthCards = baseResponse.TotalGrowthCards,
            HasLargestArmy = baseResponse.HasLargestArmy,
            HasLongestRoad = baseResponse.HasLongestRoad,
            RemainingTowns = baseResponse.RemainingTowns,
            RemainingRoads = baseResponse.RemainingRoads,
            RemainingVillages = baseResponse.RemainingVillages,
            CardsToDiscard = baseResponse.CardsToDiscard
        };
    }
}
