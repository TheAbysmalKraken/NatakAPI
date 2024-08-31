namespace Catan.Domain.UnitTests;

public sealed class CardManagerTests
{
    internal enum TestCard
    {
        TestCard1,
        TestCard2,
        TestCard3
    }

    [Fact]
    public void Add__Should_AddCard()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();

        // Act
        cardManager.Add(TestCard.TestCard1);

        // Assert
        Assert.Equal(1, cardManager.Cards[TestCard.TestCard1]);
    }

    [Fact]
    public void Add_Should_AddMultipleCards()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();

        // Act
        cardManager.Add(TestCard.TestCard1, 3);

        // Assert
        Assert.Equal(3, cardManager.Cards[TestCard.TestCard1]);
    }

    [Fact]
    public void Add_Should_NotSet()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();

        // Act
        cardManager.Add(TestCard.TestCard1, 3);
        cardManager.Add(TestCard.TestCard1, 2);

        // Assert
        Assert.Equal(5, cardManager.Cards[TestCard.TestCard1]);
    }

    [Fact]
    public void Add_Should_AddMultipleCardsToDifferentTypes()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();

        // Act
        cardManager.Add(TestCard.TestCard1, 3);
        cardManager.Add(TestCard.TestCard2, 2);

        // Assert
        Assert.Equal(3, cardManager.Cards[TestCard.TestCard1]);
        Assert.Equal(2, cardManager.Cards[TestCard.TestCard2]);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Add_Should_ThrowException_WhenCountIsLessThanOrEqualToZero(int count)
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();

        // Act
        void action() => cardManager.Add(TestCard.TestCard1, count);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void Has_Should_ReturnTrue_WhenCardExists()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();
        cardManager.Add(TestCard.TestCard1);

        // Act
        var hasCard = cardManager.Has(TestCard.TestCard1);

        // Assert
        Assert.True(hasCard);
    }

    [Fact]
    public void Has_Should_ReturnFalse_WhenCardDoesNotExist()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();

        // Act
        var hasCard = cardManager.Has(TestCard.TestCard1);

        // Assert
        Assert.False(hasCard);
    }

    [Fact]
    public void Has_Should_ReturnFalse_WhenCardCountIsZero()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();
        cardManager.Add(TestCard.TestCard1);
        cardManager.Remove(TestCard.TestCard1);

        // Act
        var hasCard = cardManager.Has(TestCard.TestCard1);

        // Assert
        Assert.False(hasCard);
    }

    [Fact]
    public void Count_Should_ReturnCardCount()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();
        cardManager.Add(TestCard.TestCard1, 3);

        // Act
        var count = cardManager.Count(TestCard.TestCard1);

        // Assert
        Assert.Equal(3, count);
    }

    [Fact]
    public void Count_Should_ReturnZero_WhenCardDoesNotExist()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();

        // Act
        var count = cardManager.Count(TestCard.TestCard1);

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void CountAll_Should_ReturnTotalCardCount()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();
        cardManager.Add(TestCard.TestCard1, 3);
        cardManager.Add(TestCard.TestCard2, 2);

        // Act
        var count = cardManager.CountAll();

        // Assert
        Assert.Equal(5, count);
    }

    [Fact]
    public void Remove_Should_RemoveCard()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();
        cardManager.Add(TestCard.TestCard1);

        // Act
        var result = cardManager.Remove(TestCard.TestCard1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, cardManager.Count(TestCard.TestCard1));
    }

    [Fact]
    public void Remove_Should_ReturnFailure_WhenCardDoesNotExist()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();

        // Act
        var result = cardManager.Remove(TestCard.TestCard1);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Remove_Should_ReturnFailure_WhenCardCountIsZero()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();
        cardManager.Add(TestCard.TestCard1);
        cardManager.Remove(TestCard.TestCard1);

        // Act
        var result = cardManager.Remove(TestCard.TestCard1);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Remove_Should_ReturnFailure_WhenCountIsGreaterThanCardCount()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();
        cardManager.Add(TestCard.TestCard1, 2);

        // Act
        var result = cardManager.Remove(TestCard.TestCard1, 3);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Remove_Should_ThrowException_WhenCountIsLessThanOrEqualToZero(int count)
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();

        // Act
        void action() => cardManager.Remove(TestCard.TestCard1, count);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void Set_Should_SetCardCount()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();
        cardManager.Add(TestCard.TestCard1);

        // Act
        cardManager.Set(TestCard.TestCard1, 3);

        // Assert
        Assert.Equal(3, cardManager.Count(TestCard.TestCard1));
    }

    [Fact]
    public void Set_Should_AddCard_WhenCardDoesNotExist()
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();

        // Act
        cardManager.Set(TestCard.TestCard1, 3);

        // Assert
        Assert.Equal(3, cardManager.Count(TestCard.TestCard1));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Set_Should_ThrowException_WhenCountIsLessThanOrEqualToZero(int count)
    {
        // Arrange
        var cardManager = new CardManager<TestCard>();

        // Act
        void action() => cardManager.Set(TestCard.TestCard1, count);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }
}
