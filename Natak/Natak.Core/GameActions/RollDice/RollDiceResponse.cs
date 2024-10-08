﻿using System.Text.Json.Serialization;

namespace Natak.Core.GameActions.RollDice;

public sealed class RollDiceResponse
{
    [JsonPropertyName("rolledDice")]
    public required IEnumerable<int> RolledDice { get; init; }

    [JsonPropertyName("rollTotal")]
    public int RollTotal => RolledDice.Sum();
}
