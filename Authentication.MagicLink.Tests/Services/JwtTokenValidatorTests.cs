namespace Authentication.MagicLink.Services;

public class JwtTokenValidatorTests
{
    private readonly ITokenValidator _jwtTokenValidator;
    private readonly ITokenGenerator _jwtTokenGenerator;

    public JwtTokenValidatorTests()
    {
        var configuration = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.test.json", false)
            .Build();

        var services = new ServiceCollection()
            .AddSingleton(configuration)
            .AddAuthenticationMagicLink(configuration)
            .BuildServiceProvider();

        _jwtTokenValidator = services.GetRequiredService<ITokenValidator>();
        _jwtTokenGenerator = services.GetRequiredService<ITokenGenerator>();
    }

    [Fact]
    public async Task ValidateToken_WithExpiredToken_ReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var token = _jwtTokenGenerator.GenerateToken(userId);
        await Task.Delay(TimeSpan.FromSeconds(10)); // Assuming token has a short expiration for testing purposes

        // Act
        var validatedUserId = _jwtTokenValidator.ValidateToken(token);

        // Assert
        validatedUserId.Should().BeNull();
    }
}