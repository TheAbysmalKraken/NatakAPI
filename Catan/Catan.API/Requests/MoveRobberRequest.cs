using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class MoveRobberRequest
{
    [JsonPropertyName("x")]
    public int? X { get; set; }

    [JsonPropertyName("y")]
    public int? Y { get; set; }
}
