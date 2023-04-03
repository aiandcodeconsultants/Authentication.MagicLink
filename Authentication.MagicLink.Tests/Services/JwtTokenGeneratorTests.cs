using System.IdentityModel.Tokens.Jwt;

namespace Authentication.MagicLink.Tests.Services;

public class JwtTokenGeneratorTests
{
    private readonly ITokenGenerator _jwtTokenGenerator;

    public JwtTokenGeneratorTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection()
            .AddAuthenticationMagicLink(configuration)
            .BuildServiceProvider();

        _jwtTokenGenerator = services.GetRequiredService<ITokenGenerator>();
    }

    [Fact]
    public void GenerateToken_WithValidUserId_ReturnsValidToken()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();

        // Act
        var token = _jwtTokenGenerator.GenerateToken(userId);
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "id");

        // Assert
        Assert.NotNull(jwtToken);
        Assert.NotNull(userIdClaim);
        Assert.Equal(userId, userIdClaim.Value);
    }

    [Fact]
    public async Task GenerateToken_WithCustomOptions_ReturnsTokenWithCorrectExpiration()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var customOptions = new MagicLinkOptions
        {
            //TokenExpirationMinutes = 5,
            TokenExpiration = TimeSpan.FromMinutes(5),
        };
        var generator = new JwtTokenGenerator(Options.Create(customOptions));

        // Act
        var token = generator.GenerateToken(userId);
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        // Assert
        Assert.NotNull(jwtToken);
        Assert.True(jwtToken.ValidTo <= DateTime.UtcNow.Add(customOptions.TokenExpiration));
    }
}
