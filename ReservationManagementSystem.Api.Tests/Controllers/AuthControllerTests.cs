using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationManagementSystem.API.Controllers;
using ReservationManagementSystem.Application.DTOs.Account;
using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Features.Users.Commands.AuthenticateUser;
using ReservationManagementSystem.Application.Features.Users.Commands.ConfirmEmail;
using ReservationManagementSystem.Application.Features.Users.Commands.ForgotPassword;
using ReservationManagementSystem.Application.Features.Users.Commands.RegisterUser;
using ReservationManagementSystem.Application.Features.Users.Commands.ResetPassword;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Api.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController(_mediatorMock.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task Authenticate_ReturnsOkResult_WithAuthenticationResponse()
    {
        // Arrange
        var request = new AuthenticateUserRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var expectedResponse = new AuthenticationResponse
        {
            Id = "ID",
            UserName = "Test",
            Email = "test@test.com",
            Roles = ["Basic"],
            JWToken = "token"
        };

        var result = Result<AuthenticationResponse>.Success(expectedResponse);

        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.Authenticate(request);

        // Assert
        var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }


    [Fact]
    public async Task Authenticate_ReturnsUnauthorized_WhenResultIsFailure()
    {
        // Arrange
        var request = new AuthenticateUserRequest
        {
            Email = "test@example.com",
            Password = "wrongpassword"
        };

        var error = new Error(ErrorType.InvalidCredentials, "Invalid credentials");
        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(Result<AuthenticationResponse>.Failure(error));

        // Act
        var result = await _controller.Authenticate(request);

        // Assert
        var actionResult = result.Result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
        var actualError = actionResult.Value.Should().BeOfType<Error>().Subject;
        actualError.Should().BeEquivalentTo(error);
    }

    [Fact]
    public async Task Register_ReturnsOkResult_WithUserId()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            UserName = "newuser",
            FirstName = "Test",
            LastName = "Test",
            Email = "newuser@example.com",
            Password = "password123",
            PasswordConfirm = "password123"
        };

        var expectedUserId = "userId123";
        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(Result<string>.Success(expectedUserId));

        // Act
        var result = await _controller.Register(request);

        // Assert
        var actionResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var actualUserId = actionResult.Value.Should().BeOfType<string>().Subject;
        actualUserId.Should().Be(expectedUserId);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenResultIsFailure()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            UserName = "newuser",
            FirstName = "Test",
            LastName = "Test",
            Email = "newuser@example.com",
            Password = "password123",
            PasswordConfirm = "password123"
        };

        var error = new Error(ErrorType.Unauthorized, "Registration failed");
        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(Result<string>.Failure(error));

        // Act
        var result = await _controller.Register(request);

        // Assert
        var actionResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var actualError = actionResult.Value.Should().BeOfType<Error>().Subject;
        actualError.Should().BeEquivalentTo(error);
    }

    [Fact]
    public async Task ConfirmEmail_ReturnsOkResult_WithConfirmationResult()
    {
        // Arrange
        var userId = "userId123";
        var code = "confirmationCode";
        var expectedConfirmationResult = "Confirmation Successful";

        _mediatorMock
            .Setup(m => m.Send(It.Is<ConfirmEmailRequest>(req => req.UserId == userId && req.Code == code), default))
            .ReturnsAsync(Result<string>.Success(expectedConfirmationResult));

        // Act
        var result = await _controller.ConfirmEmail(userId, code);

        // Assert
        var actionResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var actualResult = actionResult.Value.Should().BeOfType<string>().Subject;
        actualResult.Should().Be(expectedConfirmationResult);
    }

    [Fact]
    public async Task ConfirmEmail_ReturnsBadRequest_WhenResultIsFailure()
    {
        // Arrange
        var userId = "userId123";
        var code = "confirmationCode";
        var error = new Error(ErrorType.EmailNotSentError, "Confirmation failed");

        _mediatorMock
            .Setup(m => m.Send(It.Is<ConfirmEmailRequest>(req => req.UserId == userId && req.Code == code), default))
            .ReturnsAsync(Result<string>.Failure(error));

        // Act
        var result = await _controller.ConfirmEmail(userId, code);

        // Assert
        var actionResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var actualError = actionResult.Value.Should().BeOfType<Error>().Subject;
        actualError.Should().BeEquivalentTo(error);
    }

    [Fact]
    public async Task ForgotPassword_ReturnsOkResult_WithSuccessMessage()
    {
        // Arrange
        var request = new ForgotPasswordRequest
        {
            Email = "user@example.com"
        };

        var expectedMessage = "Password reset email sent";
        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(Result<string>.Success(expectedMessage));

        // Act
        var result = await _controller.ForgotPassword(request);

        // Assert
        var actionResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var actualMessage = actionResult.Value.Should().BeOfType<string>().Subject;
        actualMessage.Should().Be(expectedMessage);
    }

    [Fact]
    public async Task ForgotPassword_ReturnsBadRequest_WhenResultIsFailure()
    {
        // Arrange
        var request = new ForgotPasswordRequest
        {
            Email = "user@example.com"
        };

        var error = new Error(ErrorType.EmailNotSentError, "Forgot password failed");
        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(Result<string>.Failure(error));

        // Act
        var result = await _controller.ForgotPassword(request);

        // Assert
        var actionResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var actualError = actionResult.Value.Should().BeOfType<Error>().Subject;
        actualError.Should().BeEquivalentTo(error);
    }

    [Fact]
    public async Task ResetPassword_ReturnsOkResult_WithSuccessMessage()
    {
        // Arrange
        var request = new ResetPasswordRequest
        {
            Email = "user@example.com",
            Token = "resetToken",
            Password = "newPassword"
        };

        var expectedMessage = "Password reset successful";
        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(Result<string>.Success(expectedMessage));

        // Act
        var result = await _controller.ResetPassword(request);

        // Assert
        var actionResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var actualMessage = actionResult.Value.Should().BeOfType<string>().Subject;
        actualMessage.Should().Be(expectedMessage);
    }

    [Fact]
    public async Task ResetPassword_ReturnsBadRequest_WhenResultIsFailure()
    {
        // Arrange
        var request = new ResetPasswordRequest
        {
            Email = "user@example.com",
            Token = "resetToken",
            Password = "newPassword"
        };

        var error = new Error(ErrorType.EmailNotSentError, "Reset password failed");
        _mediatorMock
            .Setup(m => m.Send(request, default))
            .ReturnsAsync(Result<string>.Failure(error));

        // Act
        var result = await _controller.ResetPassword(request);

        // Assert
        var actionResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var actualError = actionResult.Value.Should().BeOfType<Error>().Subject;
        actualError.Should().BeEquivalentTo(error);

    }
}
