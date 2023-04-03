namespace Authentication.MagicLink.Models;
/// <summary>An options model for magic-link settings.</summary>
public class MagicLinkOptions
{
    /// <summary>The issuer.</summary>
    public string Issuer { get; set; } = "localhost";
    
    /// <summary>The audience.</summary>
    public string Audience { get; set; } = "localhost";
    
    /// <summary>The secret key.</summary>
    public string SecretKey { get; set; } = "your_secret_key";
    
    /// <summary>The token expiration.</summary>
    public TimeSpan TokenExpiration { get; set; } = TimeSpan.FromMinutes(5);
    
    /// <summary>The magic link base URL.</summary>
    public string MagicLinkBaseUrl { get; set; } = "https://localhost/magiclink";

    /// <summary>The email service type.</summary>
    public EmailServiceType EmailServiceType { get; set; } = EmailServiceType.Smtp;

    /// <summary>The send from address.</summary>
    public string EmailFromAddress { get; set; } = "noreply@example.com";

    /// <summary>The send from name.</summary>
    public string EmailFromName { get; set; } = "MagicLink";

    /// <summary>The SendGrid API key.</summary>
    public string SendGridApiKey { get; set; } = "<YourSendGridApiKey>";

    /// <summary>The MailKit server.</summary>
    public string MailKitServer { get; set; } = "localhost";

    /// <summary>The MailKit port.</summary>
    public int MailKitPort { get; set; } = 587;

    /// <summary>Whether MailKit should use SSL.</summary>
    public bool MailKitUseSsl { get; set; } = false;

    /// <summary>The MailKit username.</summary>
    public string MailKitUsername { get; set; } = "<YourMailKitUsername>";

    /// <summary>The MailKit password.</summary>
    public string MailKitPassword { get; set; } = "<YourMailKitPassword>";    

    /// <summary>Set to true to enable the distributed chaching for users ("redis" connection string).</summary>
    public bool UseDistributedCache { get; set; } = false;
}