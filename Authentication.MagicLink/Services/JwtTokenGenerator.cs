using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.MagicLink.Services;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly MagicLinkSettings _options;

    public JwtTokenGenerator(IOptions<MagicLinkSettings> options)
    {
        _options = options.Value;
    }

    public string GenerateToken(string userId)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_options.TokenExpirationMinutes),
            //Expires = DateTime.UtcNow.Add(_options.TokenExpiration),
            SigningCredentials = signingCredentials,
            Claims = claims.ToDictionary(c => c.Type, c => (object)c.Value),
            IssuedAt = DateTime.UtcNow,            
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }
}
