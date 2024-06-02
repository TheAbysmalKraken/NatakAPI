using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class PlayYearOfPlentyCardRequest
{
    [JsonPropertyName("firstResource")]
    public required int FirstResource { get; set; }

    [JsonPropertyName("secondResource")]
    public required int SecondResource { get; set; }
}
