using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Authentication.MagicLink.Services;

public class MailKitEmailService : IEmailService
{
    private readonly MagicLinkOptions _options;

    public MailKitEmailService(IOptions<MagicLinkOptions> options)
    {
        _options = options.Value;
    }

    //public async Task<bool> SendEmailAsync(string to, string subject, string htmlContent)
    public async Task SendEmailAsync(EmailMessage message)
    {
        var to = message.To;
        var subject = message.Subject;
        var htmlContent = message.Body;

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_options.EmailFromName, _options.EmailFromAddress));
        email.To.Add(new MailboxAddress("", to));
        email.Subject = subject;
        email.Body = new TextPart("html") { Text = htmlContent };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_options.MailKitServer, _options.MailKitPort, _options.MailKitUseSsl);
            await client.AuthenticateAsync(_options.MailKitUsername, _options.MailKitPassword);
            await client.SendAsync(email);
            await client.DisconnectAsync(true);
        }

        //return true;
    }
}
