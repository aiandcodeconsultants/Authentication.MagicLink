namespace Authentication.MagicLink.Extensions;

/// <summary>Extension methods for services implementing <see cref="ITokenValidator" />.</summary>
public static class TokenValidatorExtensions
{
    public static string? ValidateToken(this ITokenValidator tokenValidator, string token)
        => tokenValidator.ValidateToken(token, out string userId) ? null : userId;
}
