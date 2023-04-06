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
            options.SecretKey = "ThisIsS0m3K3yY0uH@v3Thrown1nH3r3";
            options.TokenExpirationMinutes = 5;
            //options.TokenExpiration = TimeSpan.FromMinutes(5);
            options.MagicLinkBaseUrl = "https://example.com/magiclink";
        });

        services.AddEmailProvider<NoSendEmailService>();
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
        //Assert.Contains(user.Id, magicLink); // NB: A token is provided, not the id, so canont check anything further
    }

    // Add more test cases for other services and components.
}
