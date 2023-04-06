using Authentication.MagicLink.Tests.Extensions;
using Authentication.MagicLink.Tests.Mocks;
//using Moq;

namespace Authentication.MagicLink.Tests.Services;

public class MagicLinkServiceTests
{
    private readonly IMagicLinkService _magicLinkService;
    private readonly IUserService _userService;
    private readonly NoSendEmailService _noSendEmailService;

    public MagicLinkServiceTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", false)
            .Build();

        var services = new ServiceCollection()
            .AddAuthenticationMagicLink(configuration)
            .AddNoSendEmailService(out _noSendEmailService)
            .BuildServiceProvider();

        _magicLinkService = services.GetRequiredService<IMagicLinkService>();
        _userService = services.GetRequiredService<IUserService>();
    }

    [Fact]
    public async Task ValidateMagicLinkAsync_WithInvalidToken_ReturnsFalse()
    {
        // Arrange
        var invalidToken = "invalid_token";

        // Act
        var result = await _magicLinkService.ValidateMagicLinkAsync(invalidToken);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateMagicLinkAsync_WithExpiredToken_ReturnsFalse()
    {
        // Arrange
        var user = await _userService.CreateUserAsync("test@example.com");
        var magicLink = await _magicLinkService.GenerateMagicLinkAsync(user.Id);
        //var token = magicLink.Substring(magicLink.LastIndexOf('/') + 1);
        var token = magicLink.Substring(magicLink.LastIndexOf("token=") + 6);

        // Wait for the token to expire (assuming it's set to a short duration for testing)
        await Task.Delay(TimeSpan.FromSeconds(10));

        // Act
        var result = await _magicLinkService.ValidateMagicLinkAsync(token);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateMagicLinkAsync_WithNewToken_ReturnsTrue()
    {
        // Arrange
        var user = await _userService.CreateUserAsync("test@example.com");
        var magicLink = await _magicLinkService.GenerateMagicLinkAsync(user.Id);
        //var token = magicLink.Substring(magicLink.LastIndexOf('/') + 1);
        var token = magicLink.Substring(magicLink.LastIndexOf("token=") + 6);

        // Act
        var result = await _magicLinkService.ValidateMagicLinkAsync(token);

        // Assert
        Assert.True(result);
    }
}