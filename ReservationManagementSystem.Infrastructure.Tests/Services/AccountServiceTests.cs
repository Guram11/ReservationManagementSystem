using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using FluentAssertions;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Domain.Settings;
using ReservationManagementSystem.Infrastructure.Identity.Models;
using ReservationManagementSystem.Infrastructure.Identity.Services;
using ReservationManagementSystem.Application.Features.Users.Commands.AuthenticateUser;
using Microsoft.AspNetCore.Http;
using ReservationManagementSystem.Application.Features.Users.Commands.RegisterUser;
using ReservationManagementSystem.Application.Features.Users.Commands.ForgotPassword;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using ReservationManagementSystem.Application.Features.Users.Commands.ConfirmEmail;
using ReservationManagementSystem.Application.Features.Users.Commands.ResetPassword;

namespace ReservationManagementSystem.Infrastructure.Tests.Services;

public class AccountServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly IOptions<JWTSettings> _jwtSettings;
    private readonly IOptions<MailSettings> _mailSettings;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>().Object;
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            store,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            new IUserValidator<ApplicationUser>[0],
            new IPasswordValidator<ApplicationUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<ApplicationUser>>().Object);

        _emailServiceMock = new Mock<IEmailService>();
        _jwtSettings = Options.Create(new JWTSettings { Key = "my_jwt_key-which-is-ultra-long", Issuer = "your_issuer",
            Audience = "your_audience", DurationInMinutes = 60 });
        _mailSettings = Options.Create(new MailSettings
        {
            SmtpHost = "localhost",
            SmtpPass = "Pass",
            SmtpPort = 123,
            SmtpUser = "User",
            DisplayName = "user",
            EmailFrom = "no-reply@yourdomain.com"
        });

        _accountService = new AccountService(
            _userManagerMock.Object,
            _jwtSettings,
            _mailSettings,
            _signInManagerMock.Object,
            _emailServiceMock.Object);
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var request = new AuthenticateUserRequest { Email = "test@test.com", Password = "password" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((ApplicationUser)null!);

        // Act
        var result = await _accountService.AuthenticateAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnFailure_WhenUserNameOrEmailIsNull()
    {
        // Arrange
        var user = new ApplicationUser { FirstName = "Test", LastName = "Test", UserName = null, Email = null };
        var request = new AuthenticateUserRequest { Email = "test@test.com", Password = "password", IpAddress = "127.0.0.1" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);

        // Act
        var result = await _accountService.AuthenticateAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnFailure_WhenPasswordSignInFails()
    {
        // Arrange
        var user = new ApplicationUser { FirstName = "Test", LastName = "Test", UserName = "testuser", Email = "test@test.com", EmailConfirmed = true };
        var request = new AuthenticateUserRequest { Email = "test@test.com", Password = "wrongpassword", IpAddress = "127.0.0.1" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.PasswordSignInAsync(user.UserName, request.Password, false, false))
                          .ReturnsAsync(SignInResult.Failed);

        // Act
        var result = await _accountService.AuthenticateAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnFailure_WhenEmailNotConfirmed()
    {
        // Arrange
        var user = new ApplicationUser { FirstName = "Test", LastName = "Test", UserName = "testuser", Email = "test@test.com", EmailConfirmed = false };
        var request = new AuthenticateUserRequest { Email = "test@test.com", Password = "password", IpAddress = "127.0.0.1" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.PasswordSignInAsync(user.UserName, request.Password, false, false))
                          .ReturnsAsync(SignInResult.Success);

        // Act
        var result = await _accountService.AuthenticateAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task AuthenticateAsync_ShouldReturnFailure_WhenTokenGenerationFails()
    {
        // Arrange
        var user = new ApplicationUser { FirstName = "Test", LastName = "Test", UserName = "testuser", Email = "test@test.com", EmailConfirmed = true };
        var request = new AuthenticateUserRequest { Email = "test@test.com", Password = "password", IpAddress = "127.0.0.1" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.PasswordSignInAsync(user.UserName, request.Password, false, false))
                          .ReturnsAsync(SignInResult.Success);

        // Act
        var result = await _accountService.AuthenticateAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnFailure_WhenUsernameIsTaken()
    {
        // Arrange
        var request = new CreateUserRequest { FirstName = "Test", LastName = "Test", UserName = "existingUser", Email = "new@test.com", Password = "password", PasswordConfirm = "password" };
        _userManagerMock.Setup(x => x.FindByNameAsync(request.UserName)).ReturnsAsync(new ApplicationUser { FirstName = "Test", LastName = "Test", UserName = request.UserName });

        // Act
        var result = await _accountService.RegisterAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnFailure_WhenEmailIsRegistered()
    {
        // Arrange
        var request = new CreateUserRequest { FirstName = "Test", LastName = "Test", UserName = "newUser", Email = "existing@test.com", Password = "password", PasswordConfirm = "password" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(new ApplicationUser { FirstName = "Test", LastName = "Test", Email = request.Email });

        // Act
        var result = await _accountService.RegisterAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnFailure_WhenPasswordsDoNotMatch()
    {
        // Arrange
        var request = new CreateUserRequest { FirstName = "Test", LastName = "Test", UserName = "newUser", Email = "test@test.com", Password = "password1", PasswordConfirm = "password2" };

        // Act
        var result = await _accountService.RegisterAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ConfirmEmailAsync_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var request = new ConfirmEmailRequest { UserId = "nonexistent", Code = "code" };
        _userManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync((ApplicationUser)null!);

        // Act
        var result = await _accountService.ConfirmEmailAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ConfirmEmailAsync_ShouldReturnFailure_WhenEmailConfirmationFails()
    {
        // Arrange
        var request = new ConfirmEmailRequest { UserId = "existingUser", Code = "code" };
        _userManagerMock.Setup(x => x.FindByIdAsync(request.UserId)).ReturnsAsync(new ApplicationUser { FirstName = "Test", LastName = "Test", });
        _userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

        // Act
        var result = await _accountService.ConfirmEmailAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ForgotPassword_ShouldReturnFailure_WhenEmailNotFound()
    {
        // Arrange
        var request = new ForgotPasswordRequest { Email = "nonexistent@test.com", Origin = "http://localhost" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((ApplicationUser)null!);

        // Act
        var result = await _accountService.ForgotPassword(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ForgotPassword_ShouldReturnSuccess_WhenEmailSent()
    {
        // Arrange
        var request = new ForgotPasswordRequest { Email = "test@test.com", Origin = "http://localhost" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(new ApplicationUser { FirstName = "Test", LastName = "Test" });
        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync("resetToken");

        // Act
        var result = await _accountService.ForgotPassword(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var request = new ResetPasswordRequest { Email = "nonexistent@test.com", Token = "token", Password = "newPassword" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((ApplicationUser)null!);

        // Act
        var result = await _accountService.ResetPassword(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnFailure_WhenPasswordResetFails()
    {
        // Arrange
        var request = new ResetPasswordRequest { Email = "test@test.com", Token = "token", Password = "newPassword" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(new ApplicationUser { FirstName = "Test", LastName = "Test", });
        _userManagerMock.Setup(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(), request.Token, request.Password))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        var result = await _accountService.ResetPassword(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnSuccess_WhenValid()
    {
        // Arrange
        var request = new ResetPasswordRequest { Email = "test@test.com", Token = "token", Password = "newPassword" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(new ApplicationUser { FirstName = "Test", LastName = "Test", });
        _userManagerMock.Setup(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(), request.Token, request.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _accountService.ResetPassword(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}