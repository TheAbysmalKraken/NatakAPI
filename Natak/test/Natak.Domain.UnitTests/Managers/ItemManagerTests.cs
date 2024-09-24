using Natak.Domain.Managers;

namespace Natak.Domain.UnitTests.Managers;

public sealed class ItemManagerTests
{
    internal enum TestItem
    {
        Item1,
        Item2,
        Item3
    }

    [Fact]
    public void Add__Should_AddItem()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();

        // Act
        itemManager.Add(TestItem.Item1);

        // Assert
        Assert.Equal(1, itemManager.Items[TestItem.Item1]);
    }

    [Fact]
    public void Add_Should_AddMultipleItems()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();

        // Act
        itemManager.Add(TestItem.Item1, 3);

        // Assert
        Assert.Equal(3, itemManager.Items[TestItem.Item1]);
    }

    [Fact]
    public void Add_Should_NotSet()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();

        // Act
        itemManager.Add(TestItem.Item1, 3);
        itemManager.Add(TestItem.Item1, 2);

        // Assert
        Assert.Equal(5, itemManager.Items[TestItem.Item1]);
    }

    [Fact]
    public void Add_Should_AddMultipleItemsToDifferentTypes()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();

        // Act
        itemManager.Add(TestItem.Item1, 3);
        itemManager.Add(TestItem.Item2, 2);

        // Assert
        Assert.Equal(3, itemManager.Items[TestItem.Item1]);
        Assert.Equal(2, itemManager.Items[TestItem.Item2]);
    }

    [Fact]
    public void Add_Should_AddMultipleItemsToDifferentTypes_WhenAddingMultipleItems()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();

        // Act
        itemManager.Add(new Dictionary<TestItem, int>
        {
            { TestItem.Item1, 3 },
            { TestItem.Item2, 2 }
        });

        // Assert
        Assert.Equal(3, itemManager.Items[TestItem.Item1]);
        Assert.Equal(2, itemManager.Items[TestItem.Item2]);
    }

    [Fact]
    public void Add_Should_ThrowException_WhenCountIsLessThanZero()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();

        // Act
        void action() => itemManager.Add(TestItem.Item1, -1);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void Has_Should_ReturnTrue_WhenItemExists()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();
        itemManager.Add(TestItem.Item1);

        // Act
        var hasItem = itemManager.Has(TestItem.Item1);

        // Assert
        Assert.True(hasItem);
    }

    [Fact]
    public void Has_Should_ReturnFalse_WhenItemDoesNotExist()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();

        // Act
        var hasItem = itemManager.Has(TestItem.Item1);

        // Assert
        Assert.False(hasItem);
    }

    [Fact]
    public void Has_Should_ReturnFalse_WhenItemCountIsZero()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();
        itemManager.Add(TestItem.Item1);
        itemManager.Remove(TestItem.Item1);

        // Act
        var hasItem = itemManager.Has(TestItem.Item1);

        // Assert
        Assert.False(hasItem);
    }

    [Fact]
    public void Count_Should_ReturnItemCount()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();
        itemManager.Add(TestItem.Item1, 3);

        // Act
        var count = itemManager.Count(TestItem.Item1);

        // Assert
        Assert.Equal(3, count);
    }

    [Fact]
    public void Count_Should_ReturnZero_WhenItemDoesNotExist()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();

        // Act
        var count = itemManager.Count(TestItem.Item1);

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void CountAll_Should_ReturnTotalItemCount()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();
        itemManager.Add(TestItem.Item1, 3);
        itemManager.Add(TestItem.Item2, 2);

        // Act
        var count = itemManager.CountAll();

        // Assert
        Assert.Equal(5, count);
    }

    [Fact]
    public void Remove_Should_RemoveItem()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();
        itemManager.Add(TestItem.Item1);

        // Act
        var result = itemManager.Remove(TestItem.Item1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, itemManager.Count(TestItem.Item1));
    }

    [Fact]
    public void Remove_Should_ReturnFailure_WhenItemDoesNotExist()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();

        // Act
        var result = itemManager.Remove(TestItem.Item1);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Remove_Should_ReturnFailure_WhenItemCountIsZero()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();
        itemManager.Add(TestItem.Item1);
        itemManager.Remove(TestItem.Item1);

        // Act
        var result = itemManager.Remove(TestItem.Item1);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Remove_Should_ReturnFailure_WhenCountIsGreaterThanItemCount()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();
        itemManager.Add(TestItem.Item1, 2);

        // Act
        var result = itemManager.Remove(TestItem.Item1, 3);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Remove_Should_ThrowException_WhenCountIsLessThanZero()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();

        // Act
        void action() => itemManager.Remove(TestItem.Item1, -1);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void Set_Should_SetItemCount()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();
        itemManager.Add(TestItem.Item1);

        // Act
        itemManager.Set(TestItem.Item1, 3);

        // Assert
        Assert.Equal(3, itemManager.Count(TestItem.Item1));
    }

    [Fact]
    public void Set_Should_AddItem_WhenItemDoesNotExist()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();

        // Act
        itemManager.Set(TestItem.Item1, 3);

        // Assert
        Assert.Equal(3, itemManager.Count(TestItem.Item1));
    }

    [Fact]
    public void Set_Should_ThrowException_WhenCountIsLessThanZero()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();

        // Act
        void action() => itemManager.Set(TestItem.Item1, -1);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void RemoveRandom_Should_RemoveRandomItem_WhenItemsAvailable()
    {
        // Arrange
        var itemManager = new ItemManager<TestItem>();
        itemManager.Add(TestItem.Item1, 1);
        itemManager.Add(TestItem.Item2, 2);
        itemManager.Add(TestItem.Item3, 3);

        // Act
        itemManager.RemoveRandom();

        // Assert
        Assert.Equal(5, itemManager.Items.Values.Sum());
    }
}
