using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ReservationManagementSystem.Application.Common.Exceptions;
using ReservationManagementSystem.Domain.Settings;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Application.DTOs.Email;
using Microsoft.Extensions.Logging;

namespace ReservationManagementSystem.Infrastructure.Identity.Services;

public class EmailService : IEmailService
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger logger;

    public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
    {
        _mailSettings = mailSettings.Value;
        this.logger = logger;
    }

    public async Task SendAsync(EmailRequest request)
    {
        try
        {
            var builder = new BodyBuilder
            {
                HtmlBody = request.Body
            };

            var email = new MimeMessage
            {
                Sender = new MailboxAddress(_mailSettings.DisplayName, request.From ?? _mailSettings.EmailFrom),
                Subject = request.Subject,
                Body = builder.ToMessageBody()
            };
            email.To.Add(MailboxAddress.Parse(request.To));

            logger.LogInformation(_mailSettings.SmtpPass);

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }
        catch (Exception ex)
        {
            throw new ApiException($"{ex.Message}, Custom error from em");
        }
    }
}
