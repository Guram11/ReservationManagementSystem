using Moq;
using FluentAssertions;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Behaviors;
using ReservationManagementSystem.Application.Wrappers;
using FluentValidation.Results;

namespace ReservationManagementSystem.Application.Tests.Common;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Should_Not_Validate_If_No_Validators()
    {
        // Arrange
        var validators = Enumerable.Empty<IValidator<TestRequest>>();
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest();
        var next = new Mock<RequestHandlerDelegate<Result<TestResponse>>>();

        next.Setup(n => n()).ReturnsAsync(Result<TestResponse>.Success(new TestResponse()));

        // Act
        var result = await behavior.Handle(request, next.Object, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        next.Verify(n => n(), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Failure_If_Validation_Fails()
    {
        // Arrange
        var validator = new Mock<IValidator<TestRequest>>();
        validator.Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
            .Returns(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required.")
            }));

        var validators = new List<IValidator<TestRequest>> { validator.Object };
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest();
        var next = new Mock<RequestHandlerDelegate<Result<TestResponse>>>();

        // Act
        var result = await behavior.Handle(request, next.Object, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("ValidationFailed");
        result.Error.Description.Should().Contain("Name is required.");
    }
}

public record TestRequest : IRequest<Result<TestResponse>>;
public record TestResponse;
