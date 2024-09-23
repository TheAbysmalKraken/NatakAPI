using System.Text.Json.Serialization;

namespace Natak.API.Requests;

public sealed class PlayYearOfPlentyCardRequest
{
    [JsonPropertyName("firstResource")]
    public required int FirstResource { get; init; }

    [JsonPropertyName("secondResource")]
    public required int SecondResource { get; init; }
}
