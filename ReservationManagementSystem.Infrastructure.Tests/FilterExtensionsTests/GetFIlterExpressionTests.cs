using ReservationManagementSystem.Infrastructure.Persistence.FilterExtensions;

namespace ReservationManagementSystem.Infrastructure.Tests.FilterExtensionsTests;

public class GetFIlterExpressionTests
{
    public class SampleEntity
    {
        public string? Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Age { get; set; }
    }

    [Fact]
    public void GetFilterExpression_WithValidStringFilter_ReturnsCorrectExpression()
    {
        // Arrange
        string filterOn = "Name";
        string filterQuery = "Test";

        // Act
        var expression = FilterExtensions.GetFilterExpression<SampleEntity>(filterOn, filterQuery);

        // Assert
        var compiledExpression = expression!.Compile();
        Assert.True(compiledExpression(new SampleEntity { Name = "Test" }));
    }

    [Fact]
    public void GetFilterExpression_WithValidIntFilter_ReturnsCorrectExpression()
    {
        // Arrange
        string filterOn = "Age";
        string filterQuery = "30";

        // Act
        var expression = FilterExtensions.GetFilterExpression<SampleEntity>(filterOn, filterQuery);

        // Assert
        var compiledExpression = expression!.Compile();
        Assert.True(compiledExpression(new SampleEntity { Age = 30 }));
        Assert.False(compiledExpression(new SampleEntity { Age = 25 }));
    }

    [Fact]
    public void GetFilterExpression_WithInvalidFilter_ReturnsNull()
    {
        // Arrange
        string filterOn = "NonExistentProperty";
        string filterQuery = "Test";

        // Act
        var expression = FilterExtensions.GetFilterExpression<SampleEntity>(filterOn, filterQuery);

        // Assert
        Assert.Null(expression);
    }
}
