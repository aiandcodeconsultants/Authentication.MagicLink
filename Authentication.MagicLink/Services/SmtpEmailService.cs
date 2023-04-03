using System.Net.Mail;

namespace Authentication.MagicLink.Services;

/// <inheritdoc cref="IEmailService"/>
public class SmtpEmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;

    /// <summary>DI constructor.</summary>
    public SmtpEmailService(SmtpClient smtpClient) => _smtpClient = smtpClient;

    /// <inheritdoc />
    public async Task SendEmailAsync(EmailMessage email)
    {
        var mailMessage = new MailMessage("noreply@example.com", email.To, email.Subject, email.Body);
        await _smtpClient.SendMailAsync(mailMessage);
    }
}
