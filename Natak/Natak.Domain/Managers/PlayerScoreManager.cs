namespace Natak.Domain.Managers;

public sealed class PlayerScoreManager
{
    private int visiblePoints = 0;
    private int hiddenPoints = 0;

    public int VisiblePoints => visiblePoints;

    public int HiddenPoints => hiddenPoints;

    public int TotalPoints => visiblePoints + hiddenPoints;

    public bool HasLargestArmy { get; private set; } = false;

    public bool HasLongestRoad { get; private set; } = false;

    public void SetHasLargestArmy(bool hasLargestArmy)
    {
        if (HasLargestArmy == hasLargestArmy)
        {
            throw new Exception("Cannot set HasLargestArmy to the same value.");
        }

        HasLargestArmy = hasLargestArmy;

        if (HasLargestArmy)
        {
            AddVisiblePoints(2);
        }
        else
        {
            RemoveVisiblePoints(2);
        }
    }

    public void SetHasLongestRoad(bool hasLongestRoad)
    {
        if (HasLongestRoad == hasLongestRoad)
        {
            throw new Exception("Cannot set HasLongestRoad to the same value.");
        }

        HasLongestRoad = hasLongestRoad;

        if (HasLongestRoad)
        {
            AddVisiblePoints(2);
        }
        else
        {
            RemoveVisiblePoints(2);
        }
    }

    public void AddVisiblePoints(int points)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(points, 0, nameof(points));

        visiblePoints += points;
    }

    public void AddHiddenPoints(int points)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(points, 0, nameof(points));

        hiddenPoints += points;
    }

    public void RemoveVisiblePoints(int points)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(points, 0, nameof(points));

        if (visiblePoints < points)
        {
            throw new Exception("Cannot remove more points than are available.");
        }

        visiblePoints -= points;
    }

    public void RemoveHiddenPoints(int points)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(points, 0, nameof(points));

        if (hiddenPoints < points)
        {
            throw new Exception("Cannot remove more points than are available.");
        }

        hiddenPoints -= points;
    }
}
