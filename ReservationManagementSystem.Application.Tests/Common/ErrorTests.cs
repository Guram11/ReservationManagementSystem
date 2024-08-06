using FluentAssertions;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Tests.Common;

public class ErrorTests
{
    [Fact]
    public void Error_Should_Create_Error_Object()
    {
        // Act
        var error = new Error("Code", "Description");

        // Assert
        error.Code.Should().Be("Code");
        error.Description.Should().Be("Description");
    }
}
