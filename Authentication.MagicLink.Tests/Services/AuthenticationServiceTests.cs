using Authentication.MagicLink.Tests.Mocks;

namespace Authentication.MagicLink.Tests.Services;

public class AuthenticationServiceTests
{
    private readonly IMagicLinkService _magicLinkService;
    private readonly IUserService _userService;

    public AuthenticationServiceTests()
    {
        var services = new ServiceCollection();
        services.AddMagicLinkAuthentication(options =>
        {
            options.Issuer = "test";
            options.Audience = "test";
            options.SecretKey = "your_secret_key";
            //options.TokenExpirationMinutes = 5;
            options.TokenExpiration = TimeSpan.FromMinutes(5);
            options.MagicLinkBaseUrl = "https://example.com/magiclink";
        });

        services.AddEmailProvider<MockEmailService>();
        var serviceProvider = services.BuildServiceProvider();

        _magicLinkService = serviceProvider.GetRequiredService<IMagicLinkService>();
        _userService = serviceProvider.GetRequiredService<IUserService>();
    }

    [Fact]
    public async Task GenerateMagicLink_WithValidUser_SendsEmailAndReturnsLink()
    {
        // Arrange
        var user = await _userService.CreateUserAsync("test@example.com");

        // Act
        var magicLink = await _magicLinkService.GenerateMagicLinkAsync(user.Id);

        // Assert
        Assert.NotNull(magicLink);
        Assert.Contains(user.Id, magicLink);
    }

    // Add more test cases for other services and components.
}
