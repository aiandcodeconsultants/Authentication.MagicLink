namespace Authentication.MagicLink.Tests.Mocks;

public class MockEmailService : IEmailService
{
    public List<EmailMessage> SentMessages { get; } = new();
    public Task SendEmailAsync(EmailMessage email)
    {
        // Log or store the email message for testing purposes.
        SentMessages.Add(email);
        // In a real-world scenario, you could use a mocking framework like Moq or NSubstitute.
        return Task.CompletedTask;
    }
}
