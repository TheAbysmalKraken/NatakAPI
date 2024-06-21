using System.Text.Json.Serialization;
using Catan.Domain.Enums;

namespace Catan.API.Requests;

public sealed class DiscardResourcesRequest
{
    [JsonPropertyName("resources")]
    public required Dictionary<ResourceType, int> Resources { get; init; }
}
