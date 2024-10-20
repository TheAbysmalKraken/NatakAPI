﻿using System.Text.Json.Serialization;
using Natak.Domain;

namespace Natak.Core.Models;

public sealed class BuildingResponse
{
    [JsonPropertyName("playerColour")]
    public required int PlayerColour { get; init; }

    [JsonPropertyName("point")]
    public required PointResponse Point { get; init; }

    public static BuildingResponse FromDomain(House house)
    {
        return new BuildingResponse
        {
            PlayerColour = (int)house.Colour,
            Point = PointResponse.FromPoint(house.Point)
        };
    }
}
