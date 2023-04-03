namespace Authentication.MagicLink.Interfaces;
/// <summary>An interface for the token-validator service.</summary>
public interface ITokenValidator
{
    /// <summary>Validates the token and returns the user identifier.</summary>
    /// <param name="token">The token.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns>True if validated.</returns>
    bool ValidateToken(string token, out string userId);
}