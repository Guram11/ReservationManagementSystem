using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ReservationManagementSystem.Application.Common.Exceptions;
using ReservationManagementSystem.Infrastructure.Identity.Interfaces;
using ReservationManagementSystem.Infrastructure.Identity.Models.Email;
using ReservationManagementSystem.Infrastructure.Identity.Settings;

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
            // create message
            var email = new MimeMessage();
            email.Sender = new MailboxAddress("Guram Chubinidze", request.From ?? "guramchubinidze@mail.com");
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = request.Body;
            email.Body = builder.ToMessageBody();
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
