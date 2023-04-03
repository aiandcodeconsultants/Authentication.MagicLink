namespace Authentication.MagicLink.Interfaces;

/// <summary>An interface for the magic-link service.</summary>
public interface IMagicLinkService
{
    /// <summary>Generates a magic link.</summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The magic link.</returns>
    Task<string> GenerateMagicLinkAsync(string userId);
    
    /// <summary>Validates a magic-link token.</summary>
    /// <param name="token">The token.</param>
    /// <returns>True if validated.</returns>
    Task<bool> ValidateMagicLinkAsync(string token);
}