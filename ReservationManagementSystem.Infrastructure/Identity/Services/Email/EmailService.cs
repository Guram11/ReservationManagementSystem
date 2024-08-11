using Microsoft.Extensions.Options;
using MimeKit;
using ReservationManagementSystem.Domain.Settings;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Application.DTOs.Email;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Infrastructure.Common;

namespace ReservationManagementSystem.Infrastructure.Identity.Services.Email;

public class EmailService : IEmailService
{
    private readonly MailSettings _mailSettings;
    private readonly IEmailSender _emailSender;

    public EmailService(IOptions<MailSettings> mailSettings, IEmailSender emailSender)
    {
        _mailSettings = mailSettings.Value;
        _emailSender = emailSender;
    }

    public async Task<Result<string>> SendAsync(EmailRequest request)
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

            await _emailSender.SendAsync(email);

            return Result<string>.Success("Email sent successfully!");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(EmailServiceErrors.EmailNotSent(ex.Message));
        }
    }
}
