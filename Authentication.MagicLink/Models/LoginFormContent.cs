namespace Authentication.MagicLink.Models;

/// <summary>A model to bind to the login form post.</summary>
public class LoginFormContent
{
    /// <summary>The email address to send to.</summary>
    public string Email { get; set; }
}
