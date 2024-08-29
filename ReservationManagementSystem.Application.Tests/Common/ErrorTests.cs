using FluentAssertions;
using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Tests.Common;

public class ErrorTests
{
    [Fact]
    public void Error_Should_Create_Error_Object()
    {
        // Act
        var error = new Error(ErrorType.None, "Description");

        // Assert
        error.ErrorType.Should().Be(ErrorType.None);
        error.Description.Should().Be("Description");
    }
}
