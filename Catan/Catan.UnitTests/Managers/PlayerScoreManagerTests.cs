using Catan.Domain.Managers;

namespace Catan.Domain.UnitTests.Managers;

public sealed class PlayerScoreManagerTests
{
    [Fact]
    public void AddVisiblePoints_Should_AddVisiblePoints()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.AddVisiblePoints(3);
        scoreManager.AddVisiblePoints(2);

        // Assert
        Assert.Equal(5, scoreManager.VisiblePoints);
    }

    [Fact]
    public void AddHiddenPoints_Should_AddHiddenPoints()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.AddHiddenPoints(3);
        scoreManager.AddHiddenPoints(2);

        // Assert
        Assert.Equal(5, scoreManager.HiddenPoints);
    }

    [Fact]
    public void TotalPoints_Should_ReturnTotalPoints()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.AddVisiblePoints(3);
        scoreManager.AddHiddenPoints(2);

        // Assert
        Assert.Equal(5, scoreManager.TotalPoints);
    }

    [Fact]
    public void SetHasLargestArmy_Should_SetHasLargestArmy()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.SetHasLargestArmy(true);

        // Assert
        Assert.True(scoreManager.HasLargestArmy);
    }

    [Fact]
    public void SetHasLongestRoad_Should_SetHasLongestRoad()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.SetHasLongestRoad(true);

        // Assert
        Assert.True(scoreManager.HasLongestRoad);
    }

    [Fact]
    public void SetHasLargestArmy_Should_AddVisiblePoints_WhenTrue()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.SetHasLargestArmy(true);

        // Assert
        Assert.Equal(2, scoreManager.VisiblePoints);
    }

    [Fact]
    public void SetHasLongestRoad_Should_AddVisiblePoints()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.SetHasLongestRoad(true);

        // Assert
        Assert.Equal(2, scoreManager.VisiblePoints);
    }

    [Fact]
    public void SetHasLargestArmy_Should_RemoveVisiblePoints_WhenFalse()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.AddVisiblePoints(5);
        scoreManager.AddHiddenPoints(3);
        scoreManager.SetHasLargestArmy(true);
        scoreManager.SetHasLargestArmy(false);

        // Assert
        Assert.Equal(5, scoreManager.VisiblePoints);
    }

    [Fact]
    public void SetHasLongestRoad_Should_RemoveVisiblePoints_WhenFalse()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.AddVisiblePoints(5);
        scoreManager.AddHiddenPoints(3);
        scoreManager.SetHasLongestRoad(true);
        scoreManager.SetHasLongestRoad(false);

        // Assert
        Assert.Equal(5, scoreManager.VisiblePoints);
    }

    [Fact]
    public void SetHasLargestArmy_Should_ThrowException_WhenSettingSameValue()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.SetHasLargestArmy(true);

        // Assert
        Assert.Throws<Exception>(() => scoreManager.SetHasLargestArmy(true));
    }

    [Fact]
    public void SetHasLongestRoad_Should_ThrowException_WhenSettingSameValue()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.SetHasLongestRoad(true);

        // Assert
        Assert.Throws<Exception>(() => scoreManager.SetHasLongestRoad(true));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void AddVisiblePoints_Should_ThrowException_WhenPointsIsLessThanOrEqualToZero(int points)
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        void action() => scoreManager.AddVisiblePoints(points);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void AddHiddenPoints_Should_ThrowException_WhenPointsIsLessThanOrEqualToZero(int points)
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        void action() => scoreManager.AddHiddenPoints(points);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void RemoveVisiblePoints_Should_RemoveVisiblePoints()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.AddVisiblePoints(5);
        scoreManager.RemoveVisiblePoints(3);

        // Assert
        Assert.Equal(2, scoreManager.VisiblePoints);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void RemoveVisiblePoints_Should_ThrowException_WhenPointsIsLessThanOrEqualToZero(int points)
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        void action() => scoreManager.RemoveVisiblePoints(points);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void RemoveVisiblePoints_Should_ThrowException_WhenPointsIsGreaterThanVisiblePoints()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        void action() => scoreManager.RemoveVisiblePoints(1);

        // Assert
        Assert.Throws<Exception>(action);
    }

    [Fact]
    public void RemoveHiddenPoints_Should_RemoveHiddenPoints()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        scoreManager.AddHiddenPoints(5);
        scoreManager.RemoveHiddenPoints(3);

        // Assert
        Assert.Equal(2, scoreManager.HiddenPoints);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void RemoveHiddenPoints_Should_ThrowException_WhenPointsIsLessThanOrEqualToZero(int points)
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        void action() => scoreManager.RemoveHiddenPoints(points);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void RemoveHiddenPoints_Should_ThrowException_WhenPointsIsGreaterThanHiddenPoints()
    {
        // Arrange
        var scoreManager = new PlayerScoreManager();

        // Act
        void action() => scoreManager.RemoveHiddenPoints(1);

        // Assert
        Assert.Throws<Exception>(action);
    }
}
