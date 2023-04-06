using System.Security.Claims;

namespace Authentication.MagicLink.Interfaces;

/// <summary>An interface for the magic-link service.</summary>
public interface IMagicLinkService
{
    /// <summary>Generates a magic link.</summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The magic link.</returns>
    Task<string> GenerateMagicLinkAsync(string userId);

    /// <summary>Validates the token and returns the claims principal for the matched user.</summary>
    /// <param name="token">The token to confirm.</param>
    /// <returns>The claims principal generated.</returns>
    Task<ClaimsPrincipal?> GetClaimsPrincipal(string token);

    /// <summary>Validates a magic-link token.</summary>
    /// <param name="token">The token.</param>
    /// <returns>True if validated.</returns>
    Task<bool> ValidateMagicLinkAsync(string token);
}