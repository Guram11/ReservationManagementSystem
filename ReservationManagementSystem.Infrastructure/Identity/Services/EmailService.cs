using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ReservationManagementSystem.Application.Common.Exceptions;
using ReservationManagementSystem.Domain.Settings;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Application.DTOs.Email;

namespace ReservationManagementSystem.Infrastructure.Identity.Services;

public class EmailService : IEmailService
{
    public MailSettings _mailSettings { get; }

    public EmailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
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
                Sender = new MailboxAddress("Guram Chubinidze", request.From ?? "guramchubinidze@mail.com"),
                Subject = request.Subject,
                Body = builder.ToMessageBody()
            };
            email.To.Add(MailboxAddress.Parse(request.To));

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("doug.vandervort54@ethereal.email", "CpCcbMUCqCUz5jxBZ3");
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

        }
        catch (Exception ex)
        {
            throw new ApiException(ex.Message);
        }
    }
}
