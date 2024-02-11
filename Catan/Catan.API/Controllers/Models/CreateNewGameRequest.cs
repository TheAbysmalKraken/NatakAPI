using System.Text.Json.Serialization;

namespace Catan.API.Controllers.Models;

public sealed class CreateNewGameRequest
{
    [JsonPropertyName("playerCount")]
    public int? PlayerCount { get; set; }
}
