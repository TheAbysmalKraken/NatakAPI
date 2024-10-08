﻿using Natak.Domain.Enums;

namespace Natak.Domain;

public class Building
{
    public Building()
    {
        Colour = PlayerColour.None;
        Type = BuildingType.None;
    }

    public Building(PlayerColour colour, BuildingType type)
    {
        Colour = colour;
        Type = type;
    }

    public PlayerColour Colour { get; protected set; }

    public BuildingType Type { get; private set; }

    public void SetColour(PlayerColour colour)
    {
        Colour = colour;
    }

    public void SetTypeToHouse()
    {
        Type = BuildingType.Village;
    }

    public void SetTypeToTown()
    {
        Type = BuildingType.Town;
    }
}
