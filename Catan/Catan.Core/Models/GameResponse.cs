using System.Text.Json.Serialization;
using Catan.Core.Mappers;
using Catan.Domain;

namespace Catan.Core.Models;

public sealed class GameResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("playerCount")]
    public required int PlayerCount { get; init; }

    [JsonPropertyName("currentPlayerColour")]
    public required int CurrentPlayerColour { get; init; }

    [JsonPropertyName("gameState")]
    public required int GameState { get; init; }

    [JsonPropertyName("actions")]
    public required List<int> Actions { get; init; }

    [JsonPropertyName("player")]
    public required DetailedPlayerResponse Player { get; init; }

    [JsonPropertyName("players")]
    public required List<PlayerResponse> Players { get; init; }

    [JsonPropertyName("board")]
    public required BoardResponse Board { get; init; }

    [JsonPropertyName("winner")]
    public required int? Winner { get; init; }

    [JsonPropertyName("lastRoll")]
    public required List<int> LastRoll { get; init; }

    [JsonPropertyName("largestArmyPlayer")]
    public required int? LargestArmyPlayer { get; init; }

    [JsonPropertyName("longestRoadPlayer")]
    public required int? LongestRoadPlayer { get; init; }

    [JsonPropertyName("remainingResourceCards")]
    public required Dictionary<int, int> RemainingResourceCards { get; init; }

    [JsonPropertyName("remainingDevelopmentCards")]
    public required int RemainingDevelopmentCards { get; init; }

    [JsonPropertyName("tradeOffer")]
    public required TradeOfferResponse TradeOffer { get; init; }

    public static GameResponse FromDomain(Game game, int playerColour)
    {
        ArgumentNullException.ThrowIfNull(game);

        var allPlayers = game.GetPlayers();
        var chosenPlayer = allPlayers.FirstOrDefault(p => (int)p.Colour == playerColour);

        ArgumentNullException.ThrowIfNull(chosenPlayer);

        return new GameResponse
        {
            Id = game.Id,
            PlayerCount = game.PlayerCount,
            CurrentPlayerColour = (int)game.CurrentPlayer.Colour,
            GameState = (int)game.CurrentState,
            Actions = game.Actions
                .Select(ActionTypeResponseMapper.FromDomain)
                .Where(action => action is not null)
                .Select(action => (int)action!)
                .Distinct()
                .ToList(),
            Player = DetailedPlayerResponse.FromDomain(chosenPlayer),
            Players = allPlayers
                .Select(PlayerResponse.FromDomain)
                .ToList(),
            Board = BoardResponse.FromDomain(game.Board),
            Winner = game.WinnerIndex != null
                ? (int)allPlayers[game.WinnerIndex.Value].Colour
                : null,
            LastRoll = game.LastRoll,
            LargestArmyPlayer = game.LargestArmyPlayerIndex != null
                ? (int)allPlayers[game.LargestArmyPlayerIndex.Value].Colour
                : null,
            LongestRoadPlayer = game.LongestRoadPlayerIndex != null
                ? (int)allPlayers[game.LongestRoadPlayerIndex.Value].Colour
                : null,
            RemainingResourceCards = game.GetRemainingResourceCards()
                .Select(kvp => new KeyValuePair<int, int>((int)kvp.Key, kvp.Value))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            RemainingDevelopmentCards = game.GetRemainingDevelopmentCards().Count,
            TradeOffer = TradeOfferResponse.FromDomain(game.TradeOffer)
        };
    }
}
