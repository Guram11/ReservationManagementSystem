using ReservationManagementSystem.Infrastructure.Persistence.FilterExtensions;

namespace ReservationManagementSystem.Infrastructure.Tests.FilterExtensionsTests;

public class ApplyPaginationTests
{
    [Fact]
    public void ApplyPagination_ReturnsPaginatedResults()
    {
        // Arrange
        var data = new List<int> { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = FilterExtensions.ApplyPagination(data, 2, 2);

        // Assert
        Assert.Equal(new List<int> { 3, 4 }, result.ToList());
    }

    [Fact]
    public void ApplyPagination_WithPageSizeLargerThanData_ReturnsRemainingResults()
    {
        // Arrange
        var data = new List<int> { 1, 2, 3, 4, 5 }.AsQueryable();

        // Act
        var result = FilterExtensions.ApplyPagination(data, 1, 10);

        // Assert
        Assert.Equal(data, result.ToList());
    }
}
