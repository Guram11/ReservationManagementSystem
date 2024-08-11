using FluentAssertions;
using Microsoft.Extensions.Options;
using MimeKit;
using Moq;
using ReservationManagementSystem.Application.DTOs.Email;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Domain.Settings;
using ReservationManagementSystem.Infrastructure.Identity.Services.Email;

namespace ReservationManagementSystem.Infrastructure.Tests;

public class EmailServiceTests
{
    private readonly Mock<IEmailSender> _mockEmailSender;
    private readonly Mock<IOptions<MailSettings>> _mockMailSettings;
    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        _mockMailSettings = new Mock<IOptions<MailSettings>>();
        _mockEmailSender = new Mock<IEmailSender>();

        var mailSettings = new MailSettings
        {
            DisplayName = "Test Sender",
            EmailFrom = "from@test.com",
            SmtpHost = "smtp.test.com",
            SmtpPort = 587,
            SmtpUser = "smtpUser",
            SmtpPass = "smtpPass"
        };

        _mockMailSettings.Setup(m => m.Value).Returns(mailSettings);

        _emailService = new EmailService(_mockMailSettings.Object, _mockEmailSender.Object);
    }

    [Fact]
    public async Task SendAsync_ShouldReturnSuccess_WhenEmailIsSentSuccessfully()
    {
        // Arrange
        var emailRequest = new EmailRequest
        {
            From = "from@test.com",
            To = "to@test.com",
            Subject = "Test Subject",
            Body = "Test Body"
        };

        _mockEmailSender.Setup(s => s.SendAsync(It.IsAny<MimeMessage>())).Returns(Task.CompletedTask);

        // Act
        var result = await _emailService.SendAsync(emailRequest);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("Email sent successfully!");
    }

    [Fact]
    public async Task SendAsync_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // Arrange
        var emailRequest = new EmailRequest
        {
            From = "from@test.com",
            To = "to@test.com",
            Subject = "Test Subject",
            Body = "Test Body"
        };

        _mockEmailSender.Setup(s => s.SendAsync(It.IsAny<MimeMessage>())).ThrowsAsync(new Exception("SMTP failure"));

        // Act
        var result = await _emailService.SendAsync(emailRequest);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}
