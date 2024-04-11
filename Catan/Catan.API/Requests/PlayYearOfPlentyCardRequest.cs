using System.Text.Json.Serialization;

namespace Catan.API.Requests;

public sealed class PlayYearOfPlentyCardRequest
{
    [JsonPropertyName("firstResourceType")]
    public int? FirstResourceType { get; set; }

    [JsonPropertyName("secondResourceType")]
    public int? SecondResourceType { get; set; }
}
