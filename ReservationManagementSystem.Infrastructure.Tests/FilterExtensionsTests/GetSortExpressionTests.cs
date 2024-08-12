using ReservationManagementSystem.Infrastructure.Persistence.FilterExtensions;

namespace ReservationManagementSystem.Infrastructure.Tests.FilterExtensionsTests;

public class GetSortExpressionTests
{
    public class SampleEntity
    {
        public string? Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Age { get; set; }
    }

    [Fact]
    public void GetSortExpression_WithValidSort_ReturnsCorrectExpression()
    {
        // Arrange
        string sortBy = "Name";

        // Act
        var expression = FilterExtensions.GetSortExpression<SampleEntity>(sortBy);

        // Assert
        var compiledExpression = expression!.Compile();
        var result = compiledExpression(new SampleEntity { Name = "Test" });

        Assert.Equal("Test", result);
    }

    [Fact]
    public void GetSortExpression_WithInvalidSort_ReturnsNull()
    {
        // Arrange
        string sortBy = "NonExistentProperty";

        // Act
        var expression = FilterExtensions.GetSortExpression<SampleEntity>(sortBy);

        // Assert
        Assert.Null(expression);
    }
}
