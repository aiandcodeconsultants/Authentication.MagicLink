namespace Authentication.MagicLink.Services;

public class UserService : IUserService
{
    // This is a simple in-memory store for demo purposes.
    // Replace it with an actual data store in production.
    private readonly Dictionary<string, User> _users = new Dictionary<string, User>();

    /// <inheritdoc />
    public Task<User?> GetUserByIdAsync(string userId)
        => Task.FromResult(_users.ContainsKey(userId) ? _users[userId] : null);

    /// <inheritdoc />
    public Task<User?> GetUserByEmailAsync(string email)
        => Task.FromResult(_users.Values.FirstOrDefault(u => u.Email == email));

    /// <inheritdoc />
    public Task<User> CreateUserAsync(string email)
    {
        var user = new User { Id = Guid.NewGuid().ToString(), Email = email };
        _users[user.Id] = user;
        return Task.FromResult(user);
    }
}
