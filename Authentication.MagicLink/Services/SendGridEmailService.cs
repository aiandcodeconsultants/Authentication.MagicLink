using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Authentication.MagicLink.Services;

public class SendGridEmailService : IEmailService
{
    private readonly MagicLinkOptions _options;

    public SendGridEmailService(IOptions<MagicLinkOptions> options)
    {
        _options = options.Value;
    }

    // public async Task<bool> SendEmailAsync(string to, string subject, string htmlContent)
    public async Task SendEmailAsync(EmailMessage message)
    {
        var to = message.To;
        var subject = message.Subject;
        var htmlContent = message.Body;

        var apiKey = _options.SendGridApiKey;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_options.EmailFromAddress, _options.EmailFromName);
        var recipient = new EmailAddress(to);
        var msg = MailHelper.CreateSingleEmail(from, recipient, subject, "", htmlContent);

        // var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        // return response.IsSuccessStatusCode;
        await client.SendEmailAsync(msg).ConfigureAwait(false);
    }
}