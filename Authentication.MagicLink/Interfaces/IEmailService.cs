namespace Authentication.MagicLink.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage email);
}