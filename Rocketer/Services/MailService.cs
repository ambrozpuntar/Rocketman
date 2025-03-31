using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Rocketer.Models.Settings;

namespace Rocketer.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using var smtpClient = new SmtpClient(_emailSettings.MailServer, _emailSettings.MailPort)
        {
            UseDefaultCredentials = false,
            Credentials = new System.Net.NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        var mailMessage = new MailMessage(
            from: new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
            to: new MailAddress(email)
        )
        {
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        await smtpClient.SendMailAsync(mailMessage);
    }
}