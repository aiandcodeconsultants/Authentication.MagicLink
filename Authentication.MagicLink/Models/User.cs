namespace Authentication.MagicLink.Models;
/// <summary>A user.</summary>
public class User
{
    /// <summary>The user identifier.</summary>
    public string Id { get; init; }
    
    /// <summary>The user email.</summary>
    public string Email { get; init; }
}