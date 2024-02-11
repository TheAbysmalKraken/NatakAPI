using System.Text.Json.Serialization;
using Catan.Domain;

namespace Catan.Application.Models;

public sealed class PlayerSpecificGameStatusResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("playerCount")]
    public int PlayerCount { get; set; }

    [JsonPropertyName("currentPlayerColour")]
    public int CurrentPlayerColour { get; set; }

    [JsonPropertyName("gamePhase")]
    public int GamePhase { get; set; }

    [JsonPropertyName("gameSubPhase")]
    public int GameSubPhase { get; set; }

    [JsonPropertyName("player")]
    public PlayerStatusResponse Player { get; set; } = new();

    [JsonPropertyName("players")]
    public List<PlayerStatusResponse> Players { get; set; } = [];

    [JsonPropertyName("board")]
    public BoardStatusResponse Board { get; set; } = new();

    [JsonPropertyName("winner")]
    public int? Winner { get; set; }

    [JsonPropertyName("lastRoll")]
    public List<int> LastRoll { get; set; } = [];

    [JsonPropertyName("largestArmyPlayer")]
    public int? LargestArmyPlayer { get; set; }

    [JsonPropertyName("longestRoadPlayer")]
    public int? LongestRoadPlayer { get; set; }

    [JsonPropertyName("remainingResourceCards")]
    public Dictionary<int, int> RemainingResourceCards { get; set; } = [];

    [JsonPropertyName("remainingDevelopmentCards")]
    public List<int> RemainingDevelopmentCards { get; set; } = [];

    public static PlayerSpecificGameStatusResponse FromDomain(CatanGame game, int playerColour)
    {
        ArgumentNullException.ThrowIfNull(game);

        var allPlayers = game.GetPlayers();
        var chosenPlayer = allPlayers.FirstOrDefault(p => (int)p.Colour == playerColour);

        ArgumentNullException.ThrowIfNull(chosenPlayer);

        return new PlayerSpecificGameStatusResponse
        {
            Id = game.Id,
            PlayerCount = game.PlayerCount,
            CurrentPlayerColour = (int)game.CurrentPlayer.Colour,
            GamePhase = (int)game.GamePhase,
            GameSubPhase = (int)game.GameSubPhase,
            Player = PlayerStatusResponse.FromDomain(chosenPlayer),
            Players = allPlayers.Select(p => PlayerStatusResponse.FromDomain(p, true)).ToList(),
            Board = BoardStatusResponse.FromDomain(game.Board),
            Winner = game.WinnerIndex != null ? (int)allPlayers[game.WinnerIndex.Value].Colour : null,
            LastRoll = game.LastRoll,
            LargestArmyPlayer = game.LargestArmyPlayerIndex != null ? (int)allPlayers[game.LargestArmyPlayerIndex.Value].Colour : null,
            LongestRoadPlayer = game.LongestRoadPlayerIndex != null ? (int)allPlayers[game.LongestRoadPlayerIndex.Value].Colour : null,
            RemainingResourceCards = game.GetRemainingResourceCards().Select(kvp => new KeyValuePair<int, int>((int)kvp.Key, kvp.Value)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            RemainingDevelopmentCards = game.GetRemainingDevelopmentCards().Select(dc => (int)dc).ToList()
        };
    }
}
