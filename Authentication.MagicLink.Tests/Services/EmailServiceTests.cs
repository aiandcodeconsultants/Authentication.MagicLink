namespace Authentication.MagicLink.Tests.Services;

public class EmailServiceTests
{
    private readonly SmtpEmailService _smtpEmailService;
    private readonly SendGridEmailService _sendGridEmailService;
    private readonly MailKitEmailService _mailKitEmailService;

    public EmailServiceTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", false)
            .Build();

        // var services = new ServiceCollection()
        //     .AddAuthenticationMagicLink(configuration)
        //     .BuildServiceProvider();

        var options = Options.Create(configuration.GetSection("MagicLink").Get<MagicLinkSettings>() ?? new());
        //var smtpOptions = Options.Create<SmtpOptions>(configuration.GetSection("MagicLink").Get<MagicLinkSettings>() ?? new());
        var host = configuration.GetValue<string>("Smtp:Host") ?? "localhost";
        var port = configuration.GetValue<int?>("Smtp:Port") ?? 25;

        // _smtpEmailService = services.GetRequiredService<IEmailService>();
        // _sendGridEmailService = services.GetRequiredService<IEmailService>();
        // _mailKitEmailService = services.GetRequiredService<IEmailService>();
        _smtpEmailService = new SmtpEmailService(new System.Net.Mail.SmtpClient(host, port));
        _sendGridEmailService = new SendGridEmailService(options);
        _mailKitEmailService = new MailKitEmailService(options);
    }

    [Theory]
    [InlineData("test@example.com", "Test Subject", "Test Body")]
    public async Task SendEmailAsync_ValidEmailData_EmailSent_MailKit(string to, string subject, string body)
    {
        // Arrange
        var emailMessage = new EmailMessage
        {
            To = to,
            Subject = subject,
            Body = body
        };

        // Act
        await _mailKitEmailService.SendEmailAsync(emailMessage);

        // Assert
    }

    [Theory]
    [InlineData("test@example.com", "Test Subject", "Test Body")]
    public async Task SendEmailAsync_ValidEmailData_EmailSent_Smtp(string to, string subject, string body)
    {
        // Arrange
        var emailMessage = new EmailMessage
        {
            To = to,
            Subject = subject,
            Body = body
        };

        // Act
        await _smtpEmailService.SendEmailAsync(emailMessage);

        // Assert
    }

    // Depending on the email service you are testing, replace _emailService with the appropriate instance
    [Theory]
    [InlineData("test@example.com", "Test Subject", "Test Body")]
    public async Task SendEmailAsync_ValidEmailData_EmailSent_SendGrid(string to, string subject, string body)
    {
        // Arrange
        var emailMessage = new EmailMessage
        {
            To = to,
            Subject = subject,
            Body = body
        };

        // Act
        await _sendGridEmailService.SendEmailAsync(emailMessage);

        // Assert
    }
}