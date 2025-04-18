﻿using System.Text.Json.Serialization;
using Natak.Domain;

namespace Natak.Core.Models;

public class HexResponse
{
    [JsonPropertyName("point")]
    public required PointResponse Point { get; init; }

    [JsonPropertyName("resource")]
    public required int Resource { get; init; }

    [JsonPropertyName("rollNumber")]
    public required int RollNumber { get; init; }

    public static HexResponse FromDomain(Tile hex)
    {
        return new HexResponse
        {
            Point = PointResponse.FromPoint(hex.Point),
            Resource = (int)hex.Type,
            RollNumber = hex.ActivationNumber
        };
    }
}
