using ReservationManagementSystem.Infrastructure.Persistence.FilterExtensions;
using System.Linq.Expressions;

namespace ReservationManagementSystem.Infrastructure.Tests.FilterExtensionsTests;

public class ApplyFilterTests
{
    [Fact]
    public void ApplyFilter_WithValidFilter_ReturnsFilteredResults()
    {
        // Arrange
        var data = new List<int> { 1, 2, 3, 4, 5 }.AsQueryable();
        Expression<Func<int, bool>> filterExpression = x => x > 3;

        // Act
        var result = data.ApplyFilter(filterExpression);

        // Assert
        Assert.Equal(new List<int> { 4, 5 }, result.ToList());
    }

    [Fact]
    public void ApplyFilter_WithNullFilter_ReturnsOriginalQuery()
    {
        // Arrange
        var data = new List<int> { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = data.ApplyFilter(null);

        // Assert
        Assert.Equal(data, result);
    }
}
