namespace Authentication.MagicLink.Tests.Services;

public class UserServiceTests
{

    private readonly IUserService _userService;

    public UserServiceTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", false)
            .Build();

        var services = new ServiceCollection()
            .AddAuthenticationMagicLink(configuration)
            .BuildServiceProvider();

        _userService = services.GetRequiredService<IUserService>();
    }

    [Fact]
    public async Task GetUserByIdAsync_NonExistingUserId_ReturnsNull()
    {
        // Arrange
        var nonExistingUserId = Guid.NewGuid().ToString();

        // Act
        var user = await _userService.GetUserByIdAsync(nonExistingUserId);

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task CreateUserAsync_CreatesUserWithCorrectEmailAndCacheEntry()
    {
        // Arrange
        var email = "test@example.com";

        // Act
        var user = await _userService.CreateUserAsync(email);
        var cachedUser = await _userService.GetUserByIdAsync(user.Id);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(email, user.Email);
        Assert.NotNull(cachedUser);
        Assert.Equal(user.Id, cachedUser.Id);
        Assert.Equal(user.Email, cachedUser.Email);
    }
}
