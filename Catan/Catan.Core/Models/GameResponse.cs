using System.Text.Json.Serialization;
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

    [JsonPropertyName("gamePhase")]
    public required int GamePhase { get; init; }

    [JsonPropertyName("gameSubPhase")]
    public required int GameSubPhase { get; init; }

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
    public required List<int> RemainingDevelopmentCards { get; init; }

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
            GamePhase = (int)game.GamePhase,
            GameSubPhase = (int)game.GameSubPhase,
            Player = DetailedPlayerResponse.FromDomain(chosenPlayer),
            Players = allPlayers
                .Select(p => PlayerResponse.FromDomain(p))
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
            RemainingDevelopmentCards = game.GetRemainingDevelopmentCards()
                .Select(dc => (int)dc)
                .ToList()
        };
    }
}
