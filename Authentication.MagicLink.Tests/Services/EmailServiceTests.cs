namespace Authentication.MagicLink.Tests.Services;

public class EmailServiceTests
{
    private readonly SmtpEmailService _smtpEmailService;
    private readonly SendGridEmailService _sendGridEmailService;
    private readonly MailKitEmailService _mailKitEmailService;

    public EmailServiceTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // var services = new ServiceCollection()
        //     .AddAuthenticationMagicLink(configuration)
        //     .BuildServiceProvider();

        var options = Options.Create<MagicLinkOptions>(configuration.GetSection("MagicLink").Get<MagicLinkOptions>() ?? new ());

        // _smtpEmailService = services.GetRequiredService<IEmailService>();
        // _sendGridEmailService = services.GetRequiredService<IEmailService>();
        // _mailKitEmailService = services.GetRequiredService<IEmailService>();
        _smtpEmailService = new SmtpEmailService(new System.Net.Mail.SmtpClient());
        _sendGridEmailService = new SendGridEmailService(options);
        _mailKitEmailService = new MailKitEmailService(options);
    }

    // Depending on the email service you are testing, replace _emailService with the appropriate instance
    [Theory]
    [InlineData("test@example.com", "Test Subject", "Test Body")]
    public async Task SendEmailAsync_ValidEmailData_EmailSent(string to, string subject, string body)
    {
        // Arrange
        var emailMessage = new EmailMessage
        {
            To = to,
            Subject = subject,
            Body = body
        };

        // Act
        /* var smtpResult = */ await _smtpEmailService.SendEmailAsync(emailMessage);
        /* var sendGridResult = */ await _sendGridEmailService.SendEmailAsync(emailMessage);
        /* var mailKitResult = */ await _mailKitEmailService.SendEmailAsync(emailMessage);

        // Assert
        // Assert.True(smtpResult);
        // Assert.True(sendGridResult);
        // Assert.True(mailKitResult);
    }
}