using ReservationManagementSystem.Infrastructure.Persistence.FilterExtensions;
using System.Linq.Expressions;

namespace ReservationManagementSystem.Infrastructure.Tests.FilterExtensionsTests;

public class ApplySortTests
{
    [Fact]
    public void ApplySort_WithValidSortAscending_ReturnsSortedResultsAscending()
    {
        // Arrange
        var data = new List<int> { 5, 1, 3, 2, 4 }.AsQueryable();
        Expression<Func<int, object>> sortExpression = x => x;

        // Act
        var result = FilterExtensions.ApplySort(data, sortExpression, true);

        // Assert
        Assert.Equal(new List<int> { 1, 2, 3, 4, 5 }, result.ToList());
    }

    [Fact]
    public void ApplySort_WithValidSortDescending_ReturnsSortedResultsDescending()
    {
        // Arrange
        var data = new List<int> { 5, 1, 3, 2, 4 }.AsQueryable();
        Expression<Func<int, object>> sortExpression = x => x;

        // Act
        var result = FilterExtensions.ApplySort(data, sortExpression, false);

        // Assert
        Assert.Equal(new List<int> { 5, 4, 3, 2, 1 }, result.ToList());
    }

    [Fact]
    public void ApplySort_WithNullSortExpression_ReturnsOriginalQuery()
    {
        // Arrange
        var data = new List<int> { 5, 1, 3, 2, 4 }.AsQueryable();

        // Act
        var result = FilterExtensions.ApplySort(data, null, true);

        // Assert
        Assert.Equal(data, result);
    }
}
