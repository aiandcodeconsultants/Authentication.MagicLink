namespace Authentication.MagicLink.Services;

public class UserService : IUserService
{
    // This is a simple in-memory store for demo purposes.
    // Replace it with an actual data store in production.
    private readonly Dictionary<string, User> _users = new Dictionary<string, User>();

    /// <inheritdoc /
    // public async Task<User> GetUserByIdAsync(string userId)
    // {
    //     return _users.ContainsKey(userId) ? _users[userId] : null;
    // }
    public Task<User?> GetUserByIdAsync(string userId)
        => Task.FromResult(_users.ContainsKey(userId) ? _users[userId] : null);

    /// <inheritdoc /
    //public async Task<User> CreateUserAsync(string email)
    public Task<User> CreateUserAsync(string email)
    {
        var user = new User { Id = Guid.NewGuid().ToString(), Email = email };
        _users[user.Id] = user;
        //return user;
        return Task.FromResult(user);
    }
}
