using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using Authentication.MagicLink.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.MagicLink.Services;

public class JwtTokenValidator : ITokenValidator
{
    private readonly MagicLinkSettings _options;

    public JwtTokenValidator(IOptions<MagicLinkSettings> options) => _options = options.Value;

    //public bool ValidateToken(string token, out string userId)
    public bool ValidateToken(string token, out string email)
    {
#nullable disable
        //userId = null;
        email = null;
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = _options.Issuer,
            ValidAudience = _options.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            //userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            //return !string.IsNullOrEmpty(userId);
            return !string.IsNullOrEmpty(email);
        }
        catch /*(Exception ex)*/
        {
            return false;
        }
#nullable enable
    }
}