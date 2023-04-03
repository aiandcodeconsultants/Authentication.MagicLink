using Microsoft.Extensions.Configuration;

namespace Authentication.MagicLink.Tests.Services;

public class MagicLinkServiceTests
{
    private readonly IMagicLinkService _magicLinkService;
    private readonly IUserService _userService;

    public MagicLinkServiceTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection()
            .AddAuthenticationMagicLink(configuration)
            .BuildServiceProvider();

        _magicLinkService = services.GetRequiredService<IMagicLinkService>();
        _userService = services.GetRequiredService<IUserService>();
    }

    [Fact]
    public async Task ValidateMagicLinkAsync_WithInvalidToken_ReturnsNull()
    {
        // Arrange
        var invalidToken = "invalid_token";

        // Act
        var result = await _magicLinkService.ValidateMagicLinkAsync(invalidToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ValidateMagicLinkAsync_WithExpiredToken_ReturnsNull()
    {
        // Arrange
        var user = await _userService.CreateUserAsync("test@example.com");
        var magicLink = await _magicLinkService.GenerateMagicLinkAsync(user.Id);
        var token = magicLink.Substring(magicLink.LastIndexOf('/') + 1);

        // Wait for the token to expire (assuming it's set to a short duration for testing)
        await Task.Delay(TimeSpan.FromSeconds(10));

        // Act
        var result = await _magicLinkService.ValidateMagicLinkAsync(token);

        // Assert
        Assert.Null(result);
    }


}