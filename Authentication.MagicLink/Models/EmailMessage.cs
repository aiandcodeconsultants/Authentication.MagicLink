namespace Authentication.MagicLink.Models;

/// <summary>An email message.</summary>
public class EmailMessage
{
    /// <summary>Who the email is to be sent to.</summary>
    public string To { get; init; }

    /// <summary>The email subject.</summary>
    public string Subject {get; init; }

    /// <summary>The email body.</summary>
    public string Body { get; init; }
}