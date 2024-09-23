namespace Natak.Domain.Factories;

internal sealed class GameFactoryOptions
{
    public int PlayerCount { get; init; } = 4;

    public bool IsSetup { get; init; } = true;

    public bool IsEndOfSetup { get; init; } = false;

    public bool GivePlayersResources { get; init; } = false;

    public bool GivePlayersDevelopmentCards { get; init; } = false;

    public bool RemovePlayersPieces { get; init; } = false;

    public bool HasRolled { get; init; } = false;

    public int? PlayersVisiblePoints { get; init; }

    public int? PlayersHiddenPoints { get; init; }

    public bool PrepareLongestRoad { get; init; } = false;

    public bool PrepareLargestArmy { get; init; } = false;

    public bool PrepareSettlementPlacement { get; init; } = false;

    public bool HasRolledSeven { get; init; } = false;
}
