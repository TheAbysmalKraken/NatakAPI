﻿using System.Text.Json.Serialization;

namespace Natak.Infrastructure.ApiResponses;

public sealed class ErrorResponse
{
    [JsonPropertyName("statusCode")]
    public required int StatusCode { get; init; }

    [JsonPropertyName("message")]
    public required string Message { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }
}
