using FluentAssertions;
using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Tests.Common;

public class ResultTests
{
    [Fact]
    public void Success_Should_Create_Successful_Result()
    {
        // Act
        var result = Result<string>.Success("Success");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Data.Should().Be("Success");
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Failure_Should_Create_Failure_Result()
    {
        // Act
        var result = Result<string>.Failure(new Error(ErrorType.None, "Error occurred"));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Data.Should().BeNull();
        result.Error.ErrorType.Should().Be(ErrorType.None);
        result.Error.Description.Should().Be("Error occurred");
    }
}
