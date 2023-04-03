namespace Authentication.MagicLink.Interfaces;
/// <summary>An interface for the token-generator service.</summary>
public interface ITokenGenerator
{
    /// <summary>Generate a token for the specified user identifier.</summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The token.</returns>
    string GenerateToken(string userId);
}
