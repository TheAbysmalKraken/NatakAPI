using System.Text.Json.Serialization;
using Natak.Domain.Enums;

namespace Natak.API.Requests;

public sealed class DiscardResourcesRequest
{
    [JsonPropertyName("resources")]
    public required Dictionary<ResourceType, int> Resources { get; init; }
}
